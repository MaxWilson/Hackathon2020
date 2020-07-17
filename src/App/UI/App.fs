module UI.App

open System
open UI
open Elmish
open Fable.SimpleHttp
open Thoth.Json
open Feliz
open Feliz.Bulma

let init _ = { error = None; whatsOnYourMind = ""; posts = [] }, Cmd.Empty
let update msg model =
    match msg with
    | Update t ->
        try
            (t model), Cmd.Empty
        with | exn ->
            { model with error = "Error during update: " + exn.ToString() |> Some }, Cmd.Empty
    | UserInput(msg) -> { model with whatsOnYourMind = msg }, Cmd.Empty
    | CreateNewPost -> 
    if model.whatsOnYourMind <> "" then 
        { model with whatsOnYourMind = ""; posts = { author = "me"; text = model.whatsOnYourMind; time = DateTime.Now }::model.posts }, Cmd.Empty
    else model, Cmd.Empty

let view model dispatch =
    match model.error with
    | Some err ->
        Bulma.section [
            Bulma.title.h1 "Something went wrong"
            Bulma.title.h3 "Catastrophic error"
            Html.div err
            ]
    | None ->
        try
            Bulma.section [
                prop.className "content"
                prop.children [
                    Bulma.content [
                        Html.form [
                            prop.onSubmit (fun ev -> ev.preventDefault(); dispatch CreateNewPost)
                            prop.children [
                                Html.textarea [prop.onChange (fun txt -> dispatch (UserInput(txt))); prop.value model.whatsOnYourMind; prop.placeholder "What's on your mind?"]
                                Html.button[prop.text "Submit"; prop.type'.submit]
                                ]
                            ]
                        for post in model.posts do
                            Html.div (sprintf "[%s] -%s at %A" post.text post.author post.time)
                        ]
                    ]
                ]
        with | exn ->
            Bulma.section [
                Bulma.title.h1 "Something went wrong"
                Bulma.title.h3 "Catastrophic error during rendering"
                Html.div (exn.ToString())
                ]