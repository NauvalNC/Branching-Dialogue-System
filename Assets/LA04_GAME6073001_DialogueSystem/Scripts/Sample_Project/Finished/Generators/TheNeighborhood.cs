#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class TheNeighborhood : StoryBuilder
    {
        public override void BuildStory()
        {
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "So here is our first stop. The most important part of you high school life.", AvatarExp.NORMAL, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "Yes, exactly. The convenience store.", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "Here you can buy snacks, drinks, and more. And by the way did I mention that you can use your school card?", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "In Katlon High, each student will be given a Katlon High Student Card or KHSC for short. You can buy anything in this city using this card with student discount.", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "If you want to recharge it, you can visit convenience store for top up.", AvatarExp.NORMAL));

            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Card? What card? I don’t have them.", AvatarExp.QUESTION, new string[] { "shiroko", "hifumi" }));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "Well, that’s true since you are new student. You will get them once you are at school.", AvatarExp.NORMAL, new string[] { "shiroko", "hifumi" }));

            Story story1 = new Story(GetNextID());
            story1.mergePointID = MAIN_STORY.GetID();
            Dialogue dialogue1 = new Dialogue("shiroko", "But for now, I’ll treat you guys a snack.", AvatarExp.HAPPY, new string[] { "shiroko", "hifumi" });
            dialogue1.branches.Add(new Branch("Thank you very much…", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Yeah! Thank you! I am glad I met a nice senpai as you.", AvatarExp.HAPPY, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "No problem. Let’s go, choose any snack you like!", AvatarExp.NORMAL));
        }
    }
}

#endif