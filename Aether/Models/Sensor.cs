﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class Sensor
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public string CrossStreet { get; set; }
        public double Distance { get; set; }

        public Sensor()
        {

        }

        public Sensor(string name, string crossStreet, double latitude, double longitude)
        {
            CrossStreet = crossStreet;
            Name = name;
            Lat = latitude;
            Long = longitude;
        }

        //might be useful later
        public static List<Sensor> Sensors = new List<Sensor>()
        {
            new Sensor("graqm0107", "Market Ave and Godfrey", 42.9547237, -85.6824347),
            new Sensor("graqm0101", "Leonard St and Monroe Ave OST",  42.984136, -85.671280),
            new Sensor("graqm0105", "B St and Godfrey", 42.9472356, -85.6822996),
            new Sensor("graqm0108", "Alger and Eastern", 42.9201462, -85.6476561),
            new Sensor("graqm0117", "Oxford and Godfrey", 42.9467373, -85.6843539),
            new Sensor("0004a30b00232915", "32nd and Broadmore", 42.904438, -85.5814071),
            new Sensor("0004a30b0023339e", "Hall and Madison", 42.9414937, -85.658029),
            new Sensor("0004a30b0023acbc", "Leonard St and Monroe Ave Simms", 42.984136, -85.671280)
        };

        public static List<Sensor> GetSensors()
        {
            List<Sensor> sensors = new List<Sensor>();
            sensors.Add(new Sensor("graqm0107", "Market Ave and Godfrey", 42.9547237, -85.6824347));
            sensors.Add(new Sensor("graqm0102", "Leonard and Remembrance", 42.987571, -85.750466));
            sensors.Add(new Sensor("graqm0106", "Hall and Godfrey", 42.9420703, -85.6847243));
            sensors.Add(new Sensor("graqm0111", "Burton and Lafayette", 42.92744, -85.6604877));
            sensors.Add(new Sensor("0004a30b00237bda", "Burton and Division", 42.927056, -85.666837));
            sensors.Add(new Sensor("0004a30b0024358c", "Eastern and Burton", 42.9273223, -85.6466512));
            sensors.Add(new Sensor("0004a30b0023339e", "Hall and Madison", 42.9414937, -85.658029));
            //sensors.Add(new Sensor("graqm0101", "Leonard St and Monroe Ave OST", 42.984136, -85.671280));
            //sensors.Add(new Sensor("graqm0105", "B St and Godfrey", 42.9472356, -85.6822996));
            //sensors.Add(new Sensor("graqm0108", "Alger and Eastern", 42.9201462, -85.6476561));
            //sensors.Add(new Sensor("graqm0117", "Oxford and Godfrey", 42.9467373, -85.6843539));
            //sensors.Add(new Sensor("0004a30b00232915", "32nd and Broadmore", 42.904438, -85.5814071));
            //sensors.Add(new Sensor("0004a30b0023acbc", "Leonard St and Monroe Ave Simms", 42.984136, -85.671280));
            return sensors;
        }
    }
}
