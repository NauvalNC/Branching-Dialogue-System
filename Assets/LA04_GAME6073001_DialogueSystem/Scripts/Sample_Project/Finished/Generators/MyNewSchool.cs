#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem_2301906331
{
    public class MyNewSchool : StoryBuilder
    {
        public override void BuildStory()
        {
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "Here new students! Welcome to Katlon High School! We will have an opening ceremony at hall at 7 A.M. So please be there on time!", AvatarExp.NORMAL, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Thank you for guiding us!", AvatarExp.HAPPY, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "No problem. I’ll leave you guys here. You can find your classroom by looking at info wall there.", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("shiroko", "I need to attend student council now. See you guys later!", AvatarExp.NORMAL, AvatarAnim.DOWN));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Bye, thank you again for guiding us!", AvatarExp.HAPPY, AvatarAnim.BOUNCE));

            Story story1 = new Story(GetNextID());
            story1.mergePointID = MAIN_STORY.GetID();
            Dialogue dialogue1 = new Dialogue("hifumi", "By the way, what class are you in?", AvatarExp.NORMAL);
            dialogue1.branches.Add(new Branch("It is LA04 - LEC", story1));
            MAIN_STORY.dialogues.Add(dialogue1);

            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Oh, I am in the same class. What a coincidence!", AvatarExp.HAPPY, AvatarAnim.BOUNCE));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Let me look where is our class at information wall.", AvatarExp.NORMAL));
            MAIN_STORY.dialogues.Add(new Dialogue("hifumi", "Hmm... ohhh... understood...", AvatarExp.NORMAL, AvatarAnim.DOWN));

            dialogue1 = new Dialogue("hifumi", "Okay, I know the direction. Should we go to our class now?", AvatarExp.NORMAL);
            dialogue1.branches.Clear();

            story1.dialogues.Add(new Dialogue("hifumi", "Okay, let’s go!", AvatarExp.HAPPY, AvatarAnim.BOUNCE));

            Story schoolStroll = new Story(GetNextID());
            schoolStroll.dialogues.Add(new Dialogue("hifumi", "Well, since we got here early, I guess why not.", AvatarExp.NORMAL));
            schoolStroll.dialogues.Add(new Dialogue("hifumi", "Let’s go for stroll a bit.", AvatarExp.HAPPY, AvatarAnim.BOUNCE));

            dialogue1.branches.Add(new Branch("Sure, let’s go!", story1, 0));
            dialogue1.branches.Add(new Branch("I think we should take a stroll", schoolStroll, 1));

            MAIN_STORY.dialogues.Add(dialogue1);
        }
    }
}

#endif