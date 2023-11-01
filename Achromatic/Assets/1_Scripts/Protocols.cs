using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols
{
    public class Packets
    {
        public class common
        {
            public int cmd;
        }
        public class req_scores : common
        {
            public string id;
            public int score;
        }

        public class res_scores : common
        {
            public string message;
        }

        public class user
        {
            public string id;
            public int score;
        }

        public class res_scores_top3 : res_scores
        {
            public user[] result;
        }

        public class res_scores_id : res_scores
        {
            public user result;
        }
    }
}