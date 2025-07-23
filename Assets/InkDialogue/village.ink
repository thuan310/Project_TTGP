
VAR VillageId = "Village"

VAR VillageState = "REQUIREMENTS_NOT_MET"



=== villageStart ===
~ StartQuest(VillageId)
#timeline: Village
Wahhh ? 
Ouch... 
#continue
Ugh… where are we 
Somewhere old… really old. I don’t recognize the exact place yet, but the air smells like wood smoke and wet soil. 
#continue
Really helpful... 
Hey, I’m working with what we’ve got
#continue
Look, there's a village nearby — let’s go there first 

~ FinishQuestStep(VillageId)
-> END

=== villageNPC1 ===
You don’t look like you're from around here. You lost, child?
I guess you could say that… What year is this?
Year? It’s the 938th year of the common era. Strange question to ask.
-> END

=== villageNPC2 ===
Your clothes look funny. Are you a time traveller?
...
Anyway, who's in charge around here?
General Ngô Quyền, of course! He’s preparing to fight the Southern Han invaders.
When I grow up I'm gonna join his force
Hey maybe you should join too
-> END

=== villageNPC3 ===
Careful near the river. The tides here are tricky - just what the General wants
Who wants what?
General Ngo Quyen, duh. He is preparing a trap for the enemies using the tides. 
Maybe you can help with that. Just join his force, it's open to all villagers
-> END

===village_discuss===
#continue
So… it’s the year 938. The Battle of Bạch Đằng.
That’s a turning point. Ngô Quyền’s victory kept the Southern Han from taking over.
Then the Villain might be trying to stop that.
Most likely. If he changes the outcome here, a lot of history could fall apart.
Then we should join Ngô Quyền. Help out.
Yes, yes. But first — you can’t walk into camp looking like that.
Right... Any ideas?
Something simple. Local. Try to blend in. And maybe… try not to get caught?
~ FinishQuestStep(VillageId)
-> END

===village_caught===
#continue
(sneaking)
#continue
Hey! What are you doing?!
Wait, I can explain—
~ FinishQuestStep(VillageId)
-> END

===village_fight===
#continue
You are really suspicuos i will fight you.
-> END

===village_end===
#continue
…Fine. You don’t seem dangerous.
#continue
If you’re serious about helping, head west. Ngô Quyền’s camp is near the bamboo grove.
Thanks.
That’s our lead. Let’s move — history won’t wait for us.
-> END













