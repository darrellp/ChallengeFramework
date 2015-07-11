module Euler40
open System
open DAP.EulerProblems.Utility

let rec firstDigitTenToN (n:int) =
    match n with
    | 0 -> 1
    | _ -> (firstDigitTenToN (n - 1)) + n * 9 * (pow 10 (n - 1))
    
let nExponent (n:int) =
    nonNegs |> Seq.map (fun n -> (n, firstDigitTenToN n)) |> truncateIf (fun (a, b) -> b > n) |> Seq.fold (fun a b -> b) (0, 0)
    
let nNum (posOrig:int) =
    let (exp, pos) = nExponent posOrig
    let tenPower = pow 10 exp
    let num = (posOrig - pos) / (exp + 1) + tenPower
    let digs = posOrig - (pos + (num - tenPower) * (exp + 1))
    (int (num.ToString().[digs])) - (int '0')
    
let answer = nonNegs |> Seq.truncate 6 |> Seq.map (fun n -> (nNum (pow 10 n))) |> Seq.fold ( * ) 1

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 40", "https://projecteuler.net/problem=40")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
210
"
#endif
