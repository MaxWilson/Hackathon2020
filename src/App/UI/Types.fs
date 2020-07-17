module UI
open System
open Myriadic

type Post = {
    time: DateTime
    text: string
    author: string
    }

[<Generator.Lenses>]
type UI = {
    error: string option
    whatsOnYourMind: string
    posts: Post list
    }

type Msg =
    | Update of transform: (UI -> UI)
    | UserInput of string
    | CreateNewPost


