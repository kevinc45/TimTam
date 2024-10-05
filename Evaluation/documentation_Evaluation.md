# Evaluation
## User Testing Goal
In this testing session, we aim to evaluate how users interact with the prototype, their reactions, and whether they can understand and complete the task.

## Evaluation Method 
We apply "think aloud" method during the testing and interview after testing. 

### Reasons for methods we apply
1. We use the think-aloud method because it allows us to gain insights into users' thought verbalizing their thoughts. This method also helps us capture real-time feedback, providing a deeper understanding of user behavior and cognitive challenges they may face.
2. We conduct interview after testing to clarify any observations and understand users in more detail. 

### Observation Tasks
1. Do users know how to interact with the prototype?
2. How do users interact with it?
3. Any difficulties?

### Interview
1.How would you describe your experience using this product?
2.What parts of the product do you think could be improved? Why?
3.Do you think this product is effective in shifting your attention away from your smartphone?
4.Can you share any difficulties you encountered?

## Outcomes
Two particiapnts join the testing. 

### Participant A:
<img width="500" alt="image" src="https://github.com/user-attachments/assets/50c682e3-5852-4d91-a9db-039d25ad77d9">

- Observation:
  -  It's quite challenging for participants to interact with the sensors using their feet to move ingredients on the screen. The participant takes so much time to try hovering on sensors.
  -  At the beginning, the participant was unaware of the correct pairing between the ingredients and the sensors.
  -  "How do I play game? Use my hands? Feet?"
  -  "It's so fun! Ohhh..I can kick it to the opposite side. But how do I know to do the next?"
  
- Interview:
  -  "The gamfication idea is cool and pretty fun that makes me stop using my smartphones. The experience is good. I can easily understand that I need to kick ingrediants, but I think the interface of menu could be improved because I would not know I need to collect ingriendt to match the menu."
  -  "When I was playing, I don't know when the game will stop."
  -  "I know you want to address the issue fo using smartphone while crossing the road, but I'm not sure it could also result in addiction, becuase I feel like I will keep playing this game even the light turns green. "
  -  "I think if there is some instruction, I will play it smoother"
 
  
    
### Participant B:
<img width="600" alt="image" src="https://github.com/user-attachments/assets/f25cdaba-72c6-4c29-b806-f39afa901fee">

- Observation:
  - "I don't know...what should I do now? Just use my feet on the sensor?"
  - "okay...I feel confused....so the thing in the middle is menu, and I need to match it, right? "
  - "How do I know I complete the task? There is no feedback for me to know, so I feel a lit bit confused."

- Interview:
  - "I'm not sure it could be effective to catch my eye because when I am on the street I would ignore this application."
  - "The one problem is that if there is no person on the opposite side, so I can't play the game right? "
  - "I think the sensors' detection sensitivity can be improved because they seem easily affected by unintended movements "

## Analysis
⁠1. Participants are confused about the interface of menu and ingredient they need to collect coz these two areas are close and users don’t know what should they do.

2.⁠ Participants may not sure what should they do for the sensor because they don’t know the connection between ingredients and sensors. 

3.⁠ ⁠⁠It’s hard for users to do calculation in their mind while playing. They need to think about the ordering of the ingredients and kick the sensor match to the ingredient.

4.⁠ ⁠⁠A problem for the scenario that what if there is no person playing a game in other side.

## Improvement
1. Only one person plays the game: We will design the mechanism to detect whether there is a person or not. And if one person is ready for playing, but the other side is no person. Then, the screen will show “Waiting for the other player…” At the same time, the panel in other side ( no player) will show up “Play me” to attract people attention to play.
2. Add the feedback for ending the game. "Time's up! Time for crossing!"
3. Imrprove the sensor sensitivity.
4. Add inital frame for the beginning of the game. 
5. Fix some bugs:⁠Ingredient do not match the recipe but still yummy, Recipe and ingredients automatically match at the beginning of the round
6. To remind users which sensor they need to interact for the ingrediants, we decided to add colour background behind the ingredients. 

