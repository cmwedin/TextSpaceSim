EXTERNAL LoadCharacter(name)
EXTERNAL GetAttrib(name)
EXTERNAL GetHealth()
EXTERNAL GetName()
EXTERNAL Damage(dmg)
EXTERNAL IsDead()

=== function CameFrom(-> x) 
	~ return TURNS_SINCE(x) == 0
	
=== function PrintNum(x) 
~ x=INT(x)
{
    - x >= 1000:
        {PrintNum(x / 1000)} thousand { x mod 1000 > 0:{PrintNum(x mod 1000)}}
    - x >= 100:
        {PrintNum(x / 100)} hundred { x mod 100 > 0:and {PrintNum(x mod 100)}}
    - x == 0:
        zero
    - else:
        { x >= 20:
            { x / 10:
                - 2: twenty
                - 3: thirty
                - 4: forty
                - 5: fifty
                - 6: sixty
                - 7: seventy
                - 8: eighty
                - 9: ninety
            }
            { x mod 10 > 0:
                <>-<>
            }
        }
        { x < 10 || x > 20:
            { x mod 10:
                - 1: one
                - 2: two
                - 3: three
                - 4: four
                - 5: five
                - 6: six
                - 7: seven
                - 8: eight
                - 9: nine
            }
        - else:
            { x:
                - 10: ten
                - 11: eleven
                - 12: twelve
                - 13: thirteen
                - 14: fourteen
                - 15: fifteen
                - 16: sixteen
                - 17: seventeen
                - 18: eighteen
                - 19: nineteen
            }
        }
}   