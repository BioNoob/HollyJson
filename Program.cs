// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using System;
using System.Text.Json.Nodes;

Console.WriteLine("Hello, World!");

string l = File.ReadAllText("C:\\Users\\bigja\\AppData\\LocalLow\\Weappy\\Holly\\Saves\\Profiles\\0\\оу.json");
var t = JsonNode.Parse(l)["stateJson"]["characters"].AsArray().Where(w => w["studioId"] != null).Where(w => w["studioId"]!.GetValue<string>() == "PL").ToList();
foreach (var item in t)
{
    var yu = item.AsObject();
    yu.Remove("limit");
    yu["limit"] = new JsonObject { []}
    var q = item?["professions"].AsObject();
}
//var y = t.Where(t => t["studioId"]!.GetValue<string>() != "EM" && t["studioId"]!.GetValue<string>() != "GB" && t["studioId"]!.GetValue<string>() != "SU" && t["studioId"]!.GetValue<string>() != "HE"
//&& t["studioId"]!.GetValue<string>() != "MA").ToList();
var tt = 0;
   