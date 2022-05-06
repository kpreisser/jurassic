using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Jurassic;
using Jurassic.Library;

static class Program
{
    static void Main()
    {
        Console.WriteLine("Framework: " + RuntimeInformation.FrameworkDescription);

        var engine = new ScriptEngine();
        var sw = Stopwatch.StartNew();

        var compiledScript = engine.Compile(new StringScriptSource(@"
""use strict"";
function doThings() {
    var _a, _b, _c, _d, _e, _f, _g, _h, _j;
    console.log(""START Function!"");
    var partitions = [];
    try {
        var documentationData = {
            project: '',
        };
        for (var _i = 0, _k = []; _i < _k.length; _i++) {
            var group = _k[_i];
            var partition = {};
            if (group.Name === 'A') {
                if (((_a = group.Values) === null || _a === void 0 ? void 0 : _a.length) > 0) {
                    for (var _l = 0, _m = group.Values; _l < _m.length; _l++) {
                        var value = _m[_l];
                        var numericValue = parseFloat(value.Value);
                        partition[value.Key] = isNaN(numericValue) ? value.Value : numericValue;
                    }
                }
                partitions.push(partition);
            }
            else if (group.Name === 'B') {
                if (((_b = group.Values) === null || _b === void 0 ? void 0 : _b.length) > 0) {
                    for (var _o = 0, _p = group.Values; _o < _p.length; _o++) {
                        var value = _p[_o];
                        if (value.Key === 'A')
                            documentationData.project = value.Value;
                        else if (value.Key === 'B')
                            documentationData.project = value.Value;
                        else if (value.Key === 'C')
                            documentationData.project = value.Value;
                        else if (value.Key === 'D')
                            documentationData.project = value.Value;
                    }
                }
            }
            else if (group.Name === 'C') {
                if (((_c = group.Values) === null || _c === void 0 ? void 0 : _c.length) > 0) {
                    for (var _q = 0, _r = group.Values; _q < _r.length; _q++) {
                        var value = _r[_q];
                        if (value.Key === 'A')
                            documentationData.project = value.Value;
                        else if (value.Key === 'B')
                            documentationData.project = value.Value;
                    }
                }
            }
            else if (group.Name === 'D') {
                if (((_d = group.Values) === null || _d === void 0 ? void 0 : _d.length) > 0) {
                    for (var _s = 0, _t = group.Values; _s < _t.length; _s++) {
                        var value = _t[_s];
                        var numericValue = parseFloat(value.Value);
                        partition[value.Key] = isNaN(numericValue) ? value.Value : numericValue;
                    }
                }
                partitions.push(partition);
            }
            else if (group.Name === 'E') {
                if (((_e = group.Values) === null || _e === void 0 ? void 0 : _e.length) > 0) {
                    for (var _u = 0, _v = group.Values; _u < _v.length; _u++) {
                        var value = _v[_u];
                        if (value.Key === 'A')
                            documentationData.project = value.Value;
                        else if (value.Key === 'B')
                            documentationData.project = value.Value;
                        else if (value.Key === 'C')
                            documentationData.project = value.Value;
                        else if (value.Key === 'D')
                            documentationData.project = value.Value;
                    }
                }
            }
            else if (group.Name === 'F') {
                if (((_f = group.Values) === null || _f === void 0 ? void 0 : _f.length) > 0) {
                    for (var _w = 0, _x = group.Values; _w < _x.length; _w++) {
                        var value = _x[_w];
                        if (value.Key === 'A')
                            documentationData.project = value.Value;
                        else if (value.Key === 'B')
                            documentationData.project = value.Value;
                    }
                }
            }
            else if (group.Name === 'G') {
                if (((_g = group.Values) === null || _g === void 0 ? void 0 : _g.length) > 0) {
                    for (var _y = 0, _z = group.Values; _y < _z.length; _y++) {
                        var value = _z[_y];
                        var numericValue = parseFloat(value.Value);
                        partition[value.Key] = isNaN(numericValue) ? value.Value : numericValue;
                    }
                }
                partitions.push(partition);
            }
            else if (group.Name === 'H') {
                if (((_h = group.Values) === null || _h === void 0 ? void 0 : _h.length) > 0) {
                    for (var _0 = 0, _1 = group.Values; _0 < _1.length; _0++) {
                        var value = _1[_0];
                        if (value.Key === 'A')
                            documentationData.project = value.Value;
                        else if (value.Key === 'B')
                            documentationData.project = value.Value;
                        else if (value.Key === 'C')
                            documentationData.project = value.Value;
                        else if (value.Key === 'D')
                            documentationData.project = value.Value;
                    }
                }
            }
            else if (group.Name === 'I') {
                if (((_j = group.Values) === null || _j === void 0 ? void 0 : _j.length) > 0) {
                    for (var _2 = 0, _3 = group.Values; _2 < _3.length; _2++) {
                        var value = _3[_2];
                        if (value.Key === 'A')
                            documentationData.project = value.Value;
                        else if (value.Key === 'B')
                            documentationData.project = value.Value;
                    }
                }
            }
        }
        partitions.sort(function (a, b) {
            if (a.X < b.X)
                return -1;
            else if (b.X < a.X)
                return 1;
            return 0;
        });
    }
    catch (ex) {
        console.logError(ex);
    }
}
try {
    console.log(""BEFORE Function"");
    var now = Date.now();
    doThings();
    console.log(""AFTER Function, execution took "" + (Date.now() - now) + "" ms"");
}
catch (e) {
    console.logError(""Exception: "" + e + (e === null || e === void 0 ? void 0 : e.stack));
}
"));

        Console.WriteLine("Compiled script after: " + sw.Elapsed);

        for (int i = 0; i < 2; i++) {
            engine = new ScriptEngine() {
                ForceStrictMode = true
            };

            engine.SetGlobalValue("console", new FirebugConsole(engine));

            sw.Restart();
            compiledScript.Execute(engine);
            sw.Stop();
            Console.WriteLine("Executed script after: " + sw.Elapsed);
        }
    }
}