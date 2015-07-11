module Euler247
open System
open System.Collections.Generic
open DAP.EulerProblems.Utility

type areaNode (ptLowerLeft:(float*float), szSquare:float, cLeft:int, cBelow:int) =
    interface IComparable with
        member this.CompareTo(an:obj) = this.szSquare.CompareTo((an :?> areaNode).szSquare)

    member this.ptLowerLeft:(float*float) = ptLowerLeft
    member this.szSquare = szSquare
    member this.cLeft = cLeft
    member this.cBelow = cBelow
    [<DefaultValue>]
    val mutable iNode:int
    member this.INode
        with get () = this.iNode
        and set newINode = 
            this.iNode <- newINode
    override this.ToString() = sprintf "Node %d with %d below and %d to the left" this.INode this.cBelow this.cLeft

    new(anParent:areaNode, fAbove:bool) =
        let ptLowerLeft =
            if fAbove then
                ((fst anParent.ptLowerLeft), snd anParent.ptLowerLeft + anParent.szSquare)
            else
                ((fst anParent.ptLowerLeft) + anParent.szSquare, (snd anParent.ptLowerLeft))
        let m = (fst ptLowerLeft) - (snd ptLowerLeft)
        let sz = (m + Math.Sqrt(m * m + 4.0))/2.0 - (fst ptLowerLeft)
        let cLeft =
            if fAbove then
                anParent.cLeft
            else
                anParent.cLeft + 1
        let cBelow =
            if fAbove then
                anParent.cBelow + 1
            else
                anParent.cBelow
        new areaNode( ptLowerLeft, sz, cLeft, cBelow)

    new() = new areaNode( (1.0, 0.0), (Math.Sqrt(5.0) - 1.0) / 2.0, 0, 0)

let s = new SortedSet<areaNode>()

let addNextNodes(iNode:int) =
    let largestNode = s.Max
    s.Remove(largestNode) |> ignore
    largestNode.iNode <- iNode
    s.Add(new areaNode(largestNode, true)) |> ignore
    s.Add(new areaNode(largestNode, false)) |> ignore
    largestNode

let anAnswer = 
    s.Add(new areaNode()) |> ignore
    nonNegs
    |> Seq.map (fun i -> addNextNodes(i))
    |> Seq.filter (fun an -> an.cBelow = 3 && an.cLeft = 3)
    // There are 20 nodes below or equal to (3,3) (6 choose 3)
    |> Seq.nth 19

let answer = anAnswer.INode

#if CHALLENGE_RUNNER
open FS_Challenges

[<Challenge("Euler Project", "Prob 247", "https://projecteuler.net/problem=247")>]
type TestChallenge() = 
    interface IChallenge with
        member this.Solve() = 
            Console.WriteLine(answer)

        member this.RetrieveSampleInput() = null
        member this.RetrieveSampleOutput() = @"
782251
"
#endif
