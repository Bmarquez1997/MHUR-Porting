﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using MHURPorting.Export;
using MHURPorting.Export.Unreal;
using MHURPorting.Services.Export;
using MHURPorting.Export.Blender;
using Newtonsoft.Json;

namespace MHURPorting.Services;

public class UnrealService : SocketServiceBase
{
    private static readonly UdpClient Client = new();

    static UnrealService()
    {
        Client.Connect("localhost", Globals.UNREAL_PORT);
    }

    public static void Send(ExportData data)
    {
        var export = new UnrealExport()
        {
            Data = data,
            AssetsRoot = App.AssetsFolder.FullName.Replace("\\", "/")
        };

        var message = JsonConvert.SerializeObject(export);
        var messageBytes = Encoding.ASCII.GetBytes(message);
        SendSpliced(Client, messageBytes, Globals.BUFFER_SIZE);
        Console.WriteLine(message);
        Client.Send(Encoding.ASCII.GetBytes("MessageFinished"));
    }
}