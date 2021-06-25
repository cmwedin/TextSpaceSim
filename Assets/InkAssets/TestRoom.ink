INCLUDE Functions.ink

This is the testing room. Over in the corner is the testing dummy. We had to do some pretty nasty things to them to get all of this you've been enjoying working.
*Talk to the testing dummy -> Testing_Dummy

=== Testing_Dummy ===
~ LoadCharacter("Testing Dummy")
The testing dummy doesnt seem to want to talk to you very much. Maybe it blames you for whats happened to it. We didnt make it so everything would be working properly for you after all.
*Examine the Dummies attributes
    ** examine its strength
    { 
        - GetAttrib("STR") < 4: 
        The dummy is very weak. 
        - GetAttrib("STR") < 7: 
        The dummy is of average strength.
        - GetAttrib("STR") < 10: 
        The dummy is considerably strong
        - GetAttrib("STR") >= 10: 
        The dummy is incrediably strong. The only reason it doesnt kill you where you stand is that combat hasnt been programed into the game yet.
        - else: Somthing went wrong, the dummy's strength should be {print_num(GetAttrib("STR"))}
    }
*Examine the Dummies health
    The Dummy has {print_num(GetHealth())} HP
- ->DONE