
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
    
    
=== bookOpened ===
Let's see what this weird book is about...
AAAAAHHHHHH—! 
Ow. That landing was less graceful than I hoped.
W-What the— Who are you?!
I could ask you the same thing! But introductions first — I’m your new spirit companion, and you just opened a very special book.
This isn’t some prank, is it?
Nope. You just triggered a time-binding portal. This book doesn’t just *tell* history — it *connects* to it.
Why would a book like that be here?
That... is a long story. But I’m here to help. You and I? We're gonna make sure history stays on track.
So, you're the one they chose?
Who are you?!
Just someone who’s tired of history always going the same way. Time for a rewrite.
No! Stop him—!
He’s trying to change the past. If we don’t follow him, everything could fall apart.
Then let’s go.
Hold on tight. This is going to get... historical.
Before we jump in, touch the book again. It’ll open the quest menu — you'll need it where we’re going.
-->END

=== openQuestMenu ===
Alright. Here goes nothing—
Deep breath! Time’s waiting!
-->END






