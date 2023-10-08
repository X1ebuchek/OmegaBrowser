﻿using System.Collections;
using System.Collections.Generic;
namespace DOM;

class DOMTokenList
{
    private IEnumerable<string> iterable;
    
    ulong length { get; }
    // string? item(ulong index);
    // bool contains(string token);
    //
    // void add(params string[] tokens);
    //
    // void remove(params string[] tokens);
    //
    // bool toggle(string token, bool force = false);
    //
    // bool replace(string token, string newToken);
    //
    // bool supports(string token);

    string value { get; }
}