module Test
open FS_Challenges
open System.IO
open System

[<Challenge("Code Chef", "Test", "https://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=7&page=show_problem&problem=442")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve(stm:System.IO.StringReader) = "F#" + Environment.NewLine
        member this.RetrieveSampleInput() = @"
input
"
        member this.RetrieveSampleOutput() = @"
F#
"
