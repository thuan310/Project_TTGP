VAR CampId = "Camp"

VAR CampState = "REQUIREMENTS_NOT_MET"



=== campStart ===
~ StartQuest(CampId)
#timeline: Camp1
...
#continue
There it is… the camp.
And look — soldiers. Training hard. This must be Ngô Quyền’s main force.
#continue
So how do we get in?
They’re focused on drills right now. If we move quietly, we might just blend in…
A stealth approach? Got it. Let’s go.
~ FinishQuestStep(CampId)

-> END

=== beingCaught ===
#continue
...
Soldier: Hey! You there!
#continue
MC: S..Sorry, I am just...
#continue
Soldier: What are you saying. Pick up a sword over there and go training.
#continue
Spirit: Maybe we should listen to him for now.
#continue
~ FinishQuestStep(CampId)

-> END

=== chatGeneral ===
#continue
Hey! You there!
Uh oh.
Stand down. I saw everything.
Oh no... busted.
#continue
Sorry for the intrusion. We just wanted to help.
You move like a scout. Not bad. But you’re clearly not from around here.
I’ve heard of your efforts, General. We want to join and support your cause.
Very brave. But that wasn't enough, mind if i have a small test on you?
-->END

=== chatGeneral2 ===
#continue
You do good on swing a sword. All right, i will let you join my force. The Southern Han will not wait.
What’s the current situation?
They’ve stationed themselves upriver. Word is, they've promoted a new general.
#continue
That must be him.
The Villain... he's hiding among them.
#continue
Possibly. Then we must be swift. We’re setting a trap — stakes beneath the tide.
Classic move. And brilliant.
But there’s still much to do. Help us prepare the stakes. The tide will soon be in our favor.
Understood. Just point me where to start.
#loadNextTutorial
-->END

=== NPC1 ===
Look at the skill. Look at the move. Soldier, what was that.
-->END

=== NPC2 ===
Strike down to the knee. He will felt down.
-->END

=== NPC3 ===
Careful. Strike straight to his heart.
-->END

=== NPC4Camp ===
Go away kid. We are talking
-->END

=== woodMaking ===
#continue
...
#continue
Not bad at all. You work fast.
Just doing what I can.
I’ve seen enough. I want your thoughts on the enemy’s next move.
Wait… are we... planning the battle?!
I guess we are.
#continue
Come, i got something to show you.
-->END

===backToCamp===
#timeline: Camp2
...
#continue
Ngô Quyền: You’ve done much already. Rest for now. Tomorrow is a big day, you need your energy for that.
Spirit: Resting before the final act… just like heroes in stories.

-->END

===ngoQuyenPhatBieu===
#timeline: Camp3
"Warriors of mine!
Ahead lies the storm, behind us — our homeland.
The enemy waits to steal all we hold dear — our land, our families, our honor.
But today… not one among us shall take a step back!
Every spear thrust, every swing of the sword, will carve our names into the eternal pages of history!
Forward! For glory, for freedom, and for our beloved homeland — forever!"
#continue
-->END
























