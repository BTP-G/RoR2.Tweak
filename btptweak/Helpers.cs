using System;
using System.Collections.Generic;
using System.Text;

namespace BtpTweak {

    internal class Helpers {

        public static string ScepterDescription(string desc) {
            return "<color=#d299ff>权杖：" + desc + "</color>";
        }

        public static string ScepterToken(string desc) {
            return "";
        }

        public static float D(float damage) {
            return 100 * damage;
        }
    }
}