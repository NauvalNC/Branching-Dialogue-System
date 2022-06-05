#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class TheEncounter : StoryBuilder
    {
        public override void BuildStory()
        {
            MAIN_STORY.dialogues.Add(new Dialogue("me", "(It is my first day of my Katlon Highschool life.)", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("me", "(Yet I lost in the middle of nowhere. I am pretty sure my GPS work perfectly, but why I didn’t charge my phone last night.)", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("me", "(Silly me...)", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("me", "(Ahh…. What should I do. I just came to this city several hours ago by bus.)", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("me", "(I should ask someone, but I am too introvert for that…)", AvatarExp.NORMAL));

            Story story1 = new Story(GetNextID());
            story1.mergePointID = MAIN_STORY.GetID();
            Dialogue dialogue1 = new Dialogue("hifumi", "Hi! I just feel like to asking this. But are you a new student?", AvatarExp.HAPPY, AvatarAnim.BOUNCE);
            dialogue1.branches.Add(new Branch("Umm.. yes, I just came here.", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Oh, so that’s true. I identify it from your school badge. I am in the same school as you are too.", AvatarExp.HAPPY, AvatarAnim.BOUNCE));
            dialogue1 = new Dialogue("hifumi", "The thing is, I want to ask you if you know the direction to school. I am just move in several hours ago, hehe.", AvatarExp.SAD, AvatarAnim.DOWN);
            dialogue1.branches.Clear();
            dialogue1.branches.Add(new Branch("Umm… well that’s my problem too.", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "You lost too! Well, it can’t be helped since we are new here.", AvatarExp.SAD));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Anyway my name is Hifumi. Nice to meet you.", AvatarExp.HAPPY));

            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "Hi guys! Are you new student here? I am part of student council of Katlon High. My name is Shiroko, I can guide you to the school.", AvatarExp.NORMAL, AvatarAnim.BOUNCE, new string[] { "hifumi", "shiroko" }));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Oh my God, seems like we got lucky. Sure and yes, we are in your care.", AvatarExp.HAPPY, AvatarAnim.BOUNCE, new string[] { "hifumi", "shiroko" }));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "Sure, no problem at all. Since you guys new here, would you like to take a walk a bit. I can introduce you some places here!", AvatarExp.NORMAL, new string[] { "hifumi", "shiroko" }));

            dialogue1 = new Dialogue("hifumi", "Oh that’s nice of you. How about you emm…?", AvatarExp.HAPPY, AvatarAnim.BOUNCE);
            dialogue1.branches.Clear();
            dialogue1.branches.Add(new Branch("Oh, my name is Player.", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            Story neighborhood = new Story(GetNextID());
            neighborhood.dialogues.Add(new Dialogue("hifumi", "Great, then Shiroko-senpai, please guide us!", AvatarExp.HAPPY, AvatarAnim.BOUNCE));
            neighborhood.dialogues.Add(new Dialogue("shiroko", "Okay, let’s go guys!", AvatarExp.NORMAL));

            Story newschool = new Story(GetNextID());
            newschool.dialogues.Add(new Dialogue("hifumi", "Well that’s actually true, maybe we can go stroll after school.", AvatarExp.NORMAL));
            newschool.dialogues.Add(new Dialogue("hifumi", "Thank you for the offer Shiroko-senpai, but I think we should go to school first.", AvatarExp.NORMAL));
            newschool.dialogues.Add(new Dialogue("shiroko", "No problem. But I might be busy after school, so I’ll give you map so you guys can go for yourself. Here.", AvatarExp.NORMAL));
            newschool.dialogues.Add(new Dialogue("hifumi", "Thank you very much.", AvatarExp.NORMAL));
            newschool.dialogues.Add(new Dialogue("shiroko", "Okay then, let’s go to school.", AvatarExp.NORMAL));

            dialogue1 = new Dialogue("hifumi", "Haha, my apologize. I forgot to ask for your name. So how about we go stroll a bit?", AvatarExp.HAPPY, AvatarAnim.DOWN);
            dialogue1.branches.Clear();
            dialogue1.branches.Add(new Branch("Sure, why not.", neighborhood, 0));
            dialogue1.branches.Add(new Branch("Umm.. it is our first day, we should go to school first", newschool, 1));
            MAIN_STORY.dialogues.Add(dialogue1);
        }
    }
}

#endif