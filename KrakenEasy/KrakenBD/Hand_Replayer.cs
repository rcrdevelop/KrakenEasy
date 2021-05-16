using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
namespace KrakenEasy.KrakenBD
{
    class Hand_Replayer
    {
        public static string[] CardSB { get; set; }
        public static string[] CardBB { get; set; }
        public static string[] CardUTG { get; set; }
        public static string[] CardUTG1 { get; set; }
        public static string[] CardMP { get; set; }
        public static string[] CardMP1 { get; set; }
        public static string[] CardCO { get; set; }
        public static string[] CardHJ { get; set; }
        public static string[] CardBTN { get; set; }
        public static List<int> BB_Restantes { get; set; }
        public static List<long> SB_Action { get; set; }
        public static List<long> BB_Action { get; set; }
        public static string[] Board { get; set; }
        public static List<BsonArray> PreFlop { get; set; }
        public static List<BsonArray> Flop { get; set; }
        public static List<BsonArray> Turn { get; set; }
        public static List<BsonArray> River { get; set; }
        public static List<BsonArray> Summary { get; set; }
    }
}
