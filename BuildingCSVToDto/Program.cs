using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CryptoVillages.Common;
using CsvHelper.Configuration;

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
            using var streamReader = new StreamReader($"/home/jaan/RiderProjects/BuildingCSVToDto/BuildingCSVToDto/{file}"); // insert csv dir here yeah?!
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
                    case "1,250":
                        temp.BuildingType = CommonCvTypes.BuildingType.Bazaar;
                        break;
                    case "2,500":
                        temp.BuildingType = CommonCvTypes.BuildingType.Bank;
                        break;
                    case "5,000":
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