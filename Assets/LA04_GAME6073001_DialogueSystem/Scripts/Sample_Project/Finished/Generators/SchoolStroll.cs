#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class SchoolStroll : StoryBuilder
    {
        public override void BuildStory()
        {
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "This school looks so nice. It’s clean and tidy.", AvatarExp.HAPPY, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "The corridor is so wide. Feels like I want to run from here there and back.", AvatarExp.HAPPY));

            Story story1 = new Story(GetNextID());
            story1.mergePointID = MAIN_STORY.GetID();

            Dialogue dialogue1 = new Dialogue("hifumi", "Hehe but of course that’s not allowed, isn’t it?", AvatarExp.NORMAL, AvatarAnim.DOWN);
            dialogue1.branches.Add(new Branch("Hifumi, look forward while you are walking!", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hoshino", "Ouch…!!", AvatarExp.SHOCKED, AvatarAnim.DOWN));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Ouch… Did I bump to something!?", AvatarExp.SHOCKED, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("hoshino", "Not something you silly! I am Hoshino, the student council president!!!", AvatarExp.ANGRY, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Waa… I am very sorry Ms. President! Umm.. Ms. President-chan?", AvatarExp.QUESTION, AvatarAnim.DOWN));
            MAIN_STORY.dialogues.Add(new Dialogue("hoshino", "Who are you calling -chan!", AvatarExp.ANGRY, AvatarAnim.BOUNCE, new string[] { "hifumi", "hoshino" }));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "I am very sorry, I apologize mam!", AvatarExp.SHOCKED, AvatarAnim.DOWN, new string[] { "hifumi", "hoshino" }));

            dialogue1 = new Dialogue("hoshino", "Jeez, new students right now have no respects.", AvatarExp.ANGRY);
            dialogue1.branches.Clear();
            dialogue1.branches.Add(new Branch("I apologize for my friend rudeness.", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hoshino", "Well, forget it. You should go to hall now. The ceremony will start soon.", AvatarExp.ANGRY));
            
            dialogue1 = new Dialogue("hoshino", "Well then, give me a room to walk!", AvatarExp.ANGRY, AvatarAnim.BOUNCE);
            dialogue1.branches.Clear();
            dialogue1.branches.Add(new Branch("Umm.. yes sure.", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            dialogue1 = new Dialogue("hifumi", "Jeez, she is completely different compared to Shiroko-senpai.", AvatarExp.ANGRY, AvatarAnim.BOUNCE);
            dialogue1.branches.Clear();
            dialogue1.branches.Add(new Branch("Sheesh.. she might hear you!", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Ooops…", AvatarExp.SHOCKED, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Anyway, we should go to hall now.", AvatarExp.NORMAL));
        }
    }
}

#endif