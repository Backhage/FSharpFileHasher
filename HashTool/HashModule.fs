module HashModule

open System.IO
open System.Security.Cryptography

type Hash (directory : string) =
    let sha256 = SHA256Managed.Create()
    let dirInfo = DirectoryInfo(directory)
    let fileInfos = dirInfo.GetFiles()

    let formatHashAsReadable hashValue =
        hashValue |> Array.fold (fun acc data -> acc + (sprintf "%02X" data)) ""

    let calcHash (fileInfo : FileInfo) =
        try
            use fileStream = fileInfo.Open(FileMode.Open)
            fileStream.Position <- 0L
            let hashValue = sha256.ComputeHash(fileStream)
            
            fileInfo.Name, formatHashAsReadable hashValue
        with
            | :? System.IO.IOException as ex -> fileInfo.Name, ex.Message
            | :? System.UnauthorizedAccessException as ex -> fileInfo.Name, ex.Message

    member this.PrintFileHashes () =
        Array.toSeq fileInfos |> Seq.map (fun file -> calcHash file) |> Seq.iter (fun (fileName, hash) -> printfn "%s: %s" fileName hash) 