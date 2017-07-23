open System.IO

[<EntryPoint>]
let main argv = 
    if Array.length argv <> 1 then
        printfn "Usage: HashTool.exe dir"
        exit 1
    
    let directory = argv.[0]

    if not <| Directory.Exists(directory) then
        printfn "ERROR: Directory '%s' does not exist" directory
        exit 1

    let hashes = HashModule.Hash directory
    hashes.PrintFileHashes ()

    0