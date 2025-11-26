# Doughmination 

A pizza shop simulator where the player is running a pizza shop. A customer will arrive every so often with their order 
above their head and the player must run around the kitchen to gather the respective ingredients, placing them on the dough.
After the order has been made, the player must pickup the pizza and hand it to the respective customer. 

The goal is to complete as many orders correctly to maintain a high store rating by the end of the day. Too many failed orders/very low
rating will result in a game over. 

## Gameplay Loop

```mermaid
flowchart TD
    A([Start Game]) --> B([Start with assembly station + ingredients])
    B --> C([Customer pulls up and specifies their order])
    C --> D([Assemble order to customer's liking])

    D --> E{Is the order correct?}
    E -->|Yes| F([Increase rating])
    E -->|No| G([Decrease rating])

    F --> C
    G --> C

    D --> H([After X orders: if rating ≥ minimum → next level])
    H -->|Otherwise| I([Game Over])


## Contributors
- Aarav Chowdhary 
- Jane Ho
- Jahnavi Panchal
- Brianna Sengchan



