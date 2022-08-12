using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CryptoVillages.Common;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace BuildingCSVtoDto
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<CommonCvTypes.Building> toRemove = new List<CommonCvTypes.Building>();
            List<CommonCvTypes.Building> buildingList = new List<CommonCvTypes.Building>();
            
            // Set up csv read
            var file = "building-purchases.csv";
            var path = "/home/jaan/RiderProjects/BuildingCSVToDto/BuildingCSVToDto/";
            using var streamReader = new StreamReader($"{path}{file}"); // insert csv dir here yeah?!
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            csvReader.Context.RegisterClassMap<BuildingDtoClassMap>();
            var buildingValue = csvReader.GetRecords<BuildingValues>();
            
            foreach (var x in buildingValue)
            {
                CommonCvTypes.Building temp = new CommonCvTypes.Building();
                temp.Owner = x.Owner;
                temp.Id = Guid.NewGuid();
                temp.LastFed = DateTime.Now;
                buildingList.Add(temp);
                switch (x.Value)
                {
                    case "50":
                        temp.BuildingType = CommonCvTypes.BuildingType.Barracks;
                        break;
                    case "100":
                        temp.BuildingType = CommonCvTypes.BuildingType.CamelStables;
                        break;
                    case "200":
                        temp.BuildingType = CommonCvTypes.BuildingType.MageAcademy;
                        break;
                    case "500":
                        temp.BuildingType = CommonCvTypes.BuildingType.TradingPost;
                        break;
                    case "1250":
                        temp.BuildingType = CommonCvTypes.BuildingType.Bazaar;
                        break;
                    case "2500":
                        temp.BuildingType = CommonCvTypes.BuildingType.Bank;
                        break;
                    case "5000":
                        temp.BuildingType = CommonCvTypes.BuildingType.Castle;
                        break;
                    default:
                        toRemove.Add(temp);
                        break;
                }
            }

            foreach (var badTx in toRemove)
            {
                buildingList.Remove(badTx);
            }

            var outputOfJsonConvert = JsonConvert.SerializeObject(buildingList, Formatting.Indented);
            File.WriteAllText($"{path}building-list.json", outputOfJsonConvert);
        }
        // Insert header names here for value mapping ease -- idk what mitch's csv looks like
        public class BuildingDtoClassMap : ClassMap<BuildingValues>
        {
            public BuildingDtoClassMap()
            {
                Map(m => m.Owner).Name("From");
                Map(m => m.Value).Name("Value");
            }
        }
        public class BuildingValues : CommonCvTypes.Building
        {
            public string Value { get; set; }
        }
    }
}