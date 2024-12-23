﻿using System.Collections.Generic;
using System.Linq;
using ClimeronToolsForPvZ.Classes.UI;
using ClimeronToolsForPvZ.Components;
using ClimeronToolsForPvZ.Extensions;
using HotKeysMod.Components;
using HotKeysMod.HotKeysCheckers;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using UnityEngine;

namespace HotKeysMod.TooltipsDrawers
{
    using ObjectType = HotKeyTooltipDrawer.ObjectType;
    public static class HotKeysTooltipsDrawerForPlantsMode
    {
        public static void CreateTooltips()
        {
            PrintPaths();
            DisableInGameTooltips();
            CreateTooltipsForCards();
            CreateTooltipsForTools();
            CreateTooltipForSlowTrigger();
            CreateTextsForGoldenBean();
        }
        private static void PrintPaths()
        {
            "Searching CardUI".Print();
            foreach (Object obj in Object.FindObjectsOfType(Il2CppType.Of<CardUI>()))
                obj.TryCast<CardUI>().transform.GetPath().Print();
            "Searching ShovelMgr".Print();
            foreach (Object obj in Object.FindObjectsOfType(Il2CppType.Of<ShovelMgr>(), true))
                obj.TryCast<ShovelMgr>().transform.GetPath().Print();
            "Searching GloveMgr".Print();
            foreach (Object obj in Object.FindObjectsOfType(Il2CppType.Of<GloveMgr>(), true))
                obj.TryCast<GloveMgr>().transform.GetPath().Print();
            "Searching HammerMgr".Print();
            foreach (Object obj in Object.FindObjectsOfType(Il2CppType.Of<HammerMgr>(), true))
                obj.TryCast<HammerMgr>().transform.GetPath().Print();
        }
        private static void CreateTooltipsForCards()
        {
            List<(KeyCode keyCode, int index)> cardsHotkeys = HotKeysManager.Cards;
            for (int i = 0; i < 14; i++)
            {
                (KeyCode keyCode, int index) tuple = cardsHotkeys.FirstOrDefault(tuple => tuple.index == i);
                if (tuple.Equals(default))
                    continue;
                Transform seed = InGameUIMgr.Instance.SeedBank.transform.Find($"SeedGroup/seed{i}");
                if (!seed || seed.childCount == 0)
                    return;
                RectTransform packet = seed.transform.GetChild(0).GetComponent<RectTransform>();
                HotKeyTooltipDrawer.CreateTooltip(packet, (char)tuple.keyCode, ObjectType.Card);
            }
        }
        private static void CreateTooltipsForTools()
        {
            CreateTooltipForTool(InGameUIMgr.Instance.ShovelBank, (char)HotKeysManager.shovel);
            CreateTooltipForTool(InGameUIMgr.Instance.GloveBank, (char)HotKeysManager.glove);
            CreateTooltipForTool(InGameUIMgr.Instance.HammerBank, (char)HotKeysManager.hammer);
        }
        private static void CreateTooltipForTool(GameObject bank, char hotkey)
        {
            if (bank == null)
                return;
            RectTransform bankRectTransform = bank.GetComponent<RectTransform>();
            HotKeyTooltipDrawer.CreateTooltip(bankRectTransform, hotkey, ObjectType.Tool);
        }
        private static void CreateTooltipForSlowTrigger()
        {
            GameObject slowTriggerObject = InGameUIMgr.Instance.SlowTrigger;
            ShadowedTextSupporter tooltip = HotKeyTooltipDrawer.CreateTooltip(slowTriggerObject.GetComponent<RectTransform>(), "Tab", ObjectType.Button);
            tooltip.Alignment = TextAlignmentOptions.Left;
        }
        private static void CreateTextsForGoldenBean()
        {
            Transform beanObject = Money.Instance.transform.Find("BeanBank");
            ShadowedTextSupporter tooltip = HotKeyTooltipDrawer.CreateTooltip(beanObject.GetComponent<RectTransform>(), (char)HotKeysManager.goldenBean, ObjectType.GoldenBean);
            tooltip.Alignment = TextAlignmentOptions.Left;
            tooltip.Size = 70;
            ShadowedTextSupporter beansAmount = ShadowedTextCreator.CreateText("BeansAmount", beanObject);
            beansAmount.Alignment = TextAlignmentOptions.Right;
            beansAmount.Color = new(0.9f, 0.9f, 0, 1);
            beansAmount.FontStyle = FontStyles.Bold;
            beansAmount.OutlineWidth = 0.3f;
            beansAmount.ShadowOffsetX = 0.03f;
            beansAmount.ShadowOffsetY = 0.02f;
            beansAmount.Size = 30;
            beansAmount.WordWrapping = false;
            RectTransform beanAmountRectTransform = beansAmount.GetComponent<RectTransform>();
            beanAmountRectTransform.localScale = Vector3.one;
            beanAmountRectTransform.localPosition = new(33, -22.5f, 0);
            BeansAmountDrawer beansAmountDrawer = beansAmount.gameObject.AddComponent<BeansAmountDrawer>();
            beansAmountDrawer.beansAmountTextSupporter = beansAmount;
        }
        private static void DisableInGameTooltips()
        {
            InGameUIMgr.Instance.ShovelBank.transform.Find("text")?.gameObject.SetActive(false);
            InGameUIMgr.Instance.ShovelBank.transform.Find("shadow")?.gameObject.SetActive(false);
            InGameUIMgr.Instance.GloveBank.transform.Find("text")?.gameObject.SetActive(false);
            InGameUIMgr.Instance.GloveBank.transform.Find("shadow")?.gameObject.SetActive(false);
            InGameUIMgr.Instance.HammerBank.transform.Find("text")?.gameObject.SetActive(false);
            InGameUIMgr.Instance.HammerBank.transform.Find("shadow")?.gameObject.SetActive(false);
            Money.Instance.beanCount2.gameObject.SetActive(false);
            GameObject slowTriggerObject = InGameUIMgr.Instance.SlowTrigger;
            foreach (Transform child in slowTriggerObject.GetComponentsInChildren<Transform>(true))
                if (child.parent == slowTriggerObject.transform && child.localPosition.y < -20)
                    child.gameObject.SetActive(false);
        }
    }
}
