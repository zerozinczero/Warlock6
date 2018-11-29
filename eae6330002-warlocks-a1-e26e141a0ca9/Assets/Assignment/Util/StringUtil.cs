using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtil {

    public static string Replace(string template, Dictionary<string,string> tokens) {
        if (tokens == null) return template;

        string s = template;
        foreach (string token in tokens.Keys) {
            string value = tokens[token];
            s = s.Replace(token, value);
        }
        return s;
    }
	
}
