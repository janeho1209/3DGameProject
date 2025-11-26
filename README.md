# Doughmination 

A pizza shop simulator where the player is running a pizza shop. A customer will arrive every so often with their order 
above their head and the player must run around the kitchen to gather the respective ingredients, placing them on the dough.
After the order has been made, the player must pickup the pizza and hand it to the respective customer. 

The goal is to complete as many orders correctly to maintain a high store rating by the end of the day. Too many failed orders/very low
rating will result in a game over. 

## Gameplay Loop

Start Game
   ↓
Start with assembly station + ingredients
   ↓
Customer specifies order
   ↓
Assemble order
   ↓
Is order correct?
   ├── Yes → Increase rating → back to next customer
   └── No  → Decrease rating → back to next customer
   ↓
After X orders:
   ├── Rating ≥ minimum → Next level (harder)
   └── Otherwise → Game Over

## Contributors
- Aarav Chowdhary 
- Jane Ho
- Jahnavi Panchal
- Brianna Sengchan

## General Goals
Jane & Brianna - Arts/Assets
    - Counter with all possible ingredients 
    - various NPCs/customer models + speech bubbles
    - Player model with uniform 
    - Customer emotes/symbols to indicate satisfaction (order correct/incorrect)
    - Will assist in Coding/Gameplay 
Aarav & Jahnavi - Coding/Gameplay 
    - Player movement 
    - Picking up/putting down ingredients on dough
    - NPC movement with speech bubble
    - Calculate order accuracy/overall rating 
    





