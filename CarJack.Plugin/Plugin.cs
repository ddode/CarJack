﻿using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using CarJack.Common;
using System.Reflection;
using BepInEx.Bootstrap;
using System.Diagnostics;

namespace CarJack.Plugin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    internal class Plugin : BaseUnityPlugin
    {
        private static Type ForceLoadCarJackCommonAssembly = typeof(DrivableCar);
        private void Awake()
        {
            Logger.LogInfo($"Loading {PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}");
            try
            {
                var directory = Path.GetDirectoryName(Info.Location);
                new CarAssets(Path.Combine(directory, "carjack"));
                CarController.Initialize(new PluginCarConfig(Config));
#if DEBUG
                CarDebugController.Create();
#endif
                CarDatabase.Initialize();
                SpawnCarApp.Initialize(directory);
                Logger.LogInfo($"Loaded {PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}!");
            }
            catch(Exception e)
            {
                Logger.LogError($"Failed to load {PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}!{Environment.NewLine}{e}");
            }
        }

        private void Start()
        {
            if (Chainloader.PluginInfos.ContainsKey("SlopCrew.Plugin"))
            {
                Logger.LogInfo("Loading CarJack SlopCrew Plugin!");
                try
                {
                    var assemblyLocation = Path.Combine(Path.GetDirectoryName(Info.Location), "CarJack.SlopCrew.dll");
                    LoadPlugin(assemblyLocation);
                }
                catch (Exception e)
                {
                    Logger.LogError($"Failed to load CarJack SlopCrew Plugin!{Environment.NewLine}{e}");
                }
            }

            if (Chainloader.PluginInfos.ContainsKey("BombRushCamera"))
            {
                Logger.LogInfo("Loading CarJack BombRushCamera Plugin!");
                try
                {
                    var assemblyLocation = Path.Combine(Path.GetDirectoryName(Info.Location), "CarJack.BombRushCamera.dll");
                    LoadPlugin(assemblyLocation);
                }
                catch (Exception e)
                {
                    Logger.LogError($"Failed to load CarJack BombRushCamera Plugin!{Environment.NewLine}{e}");
                }
            }
        }

        private void LoadPlugin(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var typesInAssembly = assembly.GetTypes();
            foreach(var type in typesInAssembly)
            {
                if (type.GetCustomAttribute<CarJackPluginAttribute>() != null)
                {
                    var instance = Activator.CreateInstance(type);
                }
            }
        }
    }
}
