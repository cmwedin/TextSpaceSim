INCLUDE Functions.ink
-(top)
{ This is the testing room. Over in the corner is the testing dummy. We had to do some pretty nasty things to them to get all of this you've been enjoying working. | The only thing in the testing room is the dummy right now }
+Talk to the testing dummy -> Testing_Dummy

=== Testing_Dummy ===
~ LoadCharacter("Testing Dummy")
{IsDead() : ->dead}
{The testing dummy doesnt seem to want to talk to you very much. Maybe it blames you for whats happened to it. We did make it so everything would be working properly for you after all. | the testing dummy continues to be inscrutable }
+Examine the Dummies attributes
    ++ examine its strength
    { 
        - GetAttrib("STR") < 4: 
        The dummy is very weak. 
        - GetAttrib("STR") < 7: 
        The dummy is of average strength.
        - GetAttrib("STR") < 10: 
        The dummy is considerably strong
        - GetAttrib("STR") >= 10: 
        The dummy is incrediably strong. The only reason it doesnt kill you where you stand is that combat hasnt been programed into the game yet.
        - else: Somthing went wrong, the dummy's strength should be {PrintNum("STR")}
    }
    ++ stop examining its attributes
    ->Testing_Dummy
+(health) Examine the Dummies health
    The Dummy has {PrintNum(GetHealth())} HP
    ++(punch) Punch The dummy
    {Damage(2)} You give the testing dummy a solid punch,<> 
    {IsDead() : and with that it collapses to the ground->dead}
    ->health
    ++Stop examining health
    ->Testing_Dummy
+ Leave the dummy alone.
    ->top
- -> Testing_Dummy
=dead
{ stopping:
- The testing dummy's body is in front of you. {CameFrom(->punch): "Was is really alive in the first place?"  you ask yourself in an attempt to assuage a nagging sense of guilt}
- The dummy is still dead
}
- ->DONE