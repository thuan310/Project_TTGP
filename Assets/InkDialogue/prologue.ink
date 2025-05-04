
VAR PrologueId = "Prologue"

VAR PrologueState = "REQUIREMENTS_NOT_MET"



=== prologueStart ===

{ PrologueState:
    - "CAN_START": -> start
    - else: -> default
}

= start
("Ugh... How long was I out?")
("Still so much to study... Maybe just one more book.")    
~ StartQuest(PrologueId)
-> END

= default
-> END

=== getBook ===
(What book should I take?)
* [Take a random book]
    (Huh, what's this?)
    (Never seen this book before)
    ~ FinishQuestStep(PrologueId)
    
* [Come back later]
    (Maybe I'll comeback later)
    - -> END