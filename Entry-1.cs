using Harmony;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CyclopsAlarmsNoMore 
{
    [QModCore]
    public class Entry
    {
        [QModPatch]
        public static void Patch()
        {
            try
            {
                HarmonyInstance.Create("Cookie.CyclopsAlarmsNoMore").PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    [HarmonyPatch(typeof(SubFloodAlarm))]
    [HarmonyPatch(nameof(SubFloodAlarm.NewAlarmState))]
    class SubFloodAlarm_NewAlarmState_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(SubFloodAlarm __instance)
        {
            __instance.sub.fireSuppressionState = false;
            __instance.sub.subWarning = false;
        }
    }

    [HarmonyPatch(typeof(CyclopsHelmHUDManager))]
    [HarmonyPatch(nameof(CyclopsHelmHUDManager.Update))]
    class CyclopsHelmHUDManager_Update_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(CyclopsHelmHUDManager __instance)
        {
            __instance.fireWarning = false;
            __instance.creatureAttackWarning = false;
            __instance.hullDamageWarning = false;
        }
    }

    [HarmonyPatch(typeof(VoiceNotificationManager))]
    [HarmonyPatch(nameof(VoiceNotificationManager.PlayVoiceNotification))]
    class VoiceNotificationManager_PlayVoiceNotification_Patch
    {
        public static List<VoiceNotification> disabledVoiceNotifications = new List<VoiceNotification>();

        [HarmonyPrefix]
        public static bool Prefix(VoiceNotification vo)
        {
            if (disabledVoiceNotifications.Count == 0)
            {
                SubRoot subRoot = Player.main.GetCurrentSub();
                if (subRoot != null)
                {
                    disabledVoiceNotifications.Add(subRoot.abandonShipNotification);
                    disabledVoiceNotifications.Add(subRoot.cavitatingNotification);
                    disabledVoiceNotifications.Add(subRoot.creatureAttackNotification);
                    disabledVoiceNotifications.Add(subRoot.engineOverheatCriticalNotification);
                    disabledVoiceNotifications.Add(subRoot.engineOverheatNotification);
                    disabledVoiceNotifications.Add(subRoot.fireNotification);
                    disabledVoiceNotifications.Add(subRoot.fireSupressionNotification);
                    disabledVoiceNotifications.Add(subRoot.hullBreachNotification);
                    disabledVoiceNotifications.Add(subRoot.hullCriticalNotification);
                    disabledVoiceNotifications.Add(subRoot.hullDamageNotification);
                    disabledVoiceNotifications.Add(subRoot.hullLowNotification);
                    disabledVoiceNotifications.Add(subRoot.welcomeNotificationEmergency);
                    disabledVoiceNotifications.Add(subRoot.welcomeNotificationIssue);
                    disabledVoiceNotifications.Add(subRoot.noPowerNotification);
                    disabledVoiceNotifications.Add(subRoot.fireExtinguishedNotification);
                    disabledVoiceNotifications.Add(subRoot.hullRestoredNotification);



                }
            }

            if(disabledVoiceNotifications.Contains(vo)) return false;

            return true;
        }
    }
}
