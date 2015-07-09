module Test
open System
let main =
    let mutable chk=true
    while chk do
        let x = int(Console.ReadLine())
        match x with
            | 42 -> chk<-false
            | _ -> printfn "%d" x
main

#if CHALLENGE_RUNNER
open System.IO
open FS_Challenges

[<Challenge("Code Chef", "Test - FS", "http://www.codechef.com/problems/TEST")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve(stm:System.IO.StringReader) = 
            Console.SetIn stm
            let sw = new StringWriter()
            Console.SetOut sw
            main
            sw.ToString();
        member this.RetrieveSampleInput() = @"
1
2
88
42
99
"
        member this.RetrieveSampleOutput() = @"
1
2
88
"
#endif

