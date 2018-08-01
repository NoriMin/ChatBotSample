using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.QnABot
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private List<string> menuList = new List<string>() { "社内手続きに関する問い合わせ", "終了" };
        private List<string> feedbackList = new List<string>() { "はい", "いいえ" };
        
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(HelloMessage);

            return Task.CompletedTask;
        }
        
        private async Task HelloMessage(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("こんにちは！私はPoC Botです。どのようなご用件でしょうか？ ");

            MenuMessage(context);
        }

        private async Task InputMessage(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("フィードバックありがとうございます。今後の精度改善の参考にさせて頂きます。");

            MenuMessage(context);
        }

        private void MenuMessage(IDialogContext context)
        {
            PromptDialog.Choice(context, SelectDialog, menuList, " 以下から選択してください。 ");
        }

        private async Task SelectDialog(IDialogContext context, IAwaitable<object> result)
        {
            var selectedMenu = await result;

            if((string)selectedMenu == "社内手続きに関する問い合わせ")
            {
                await context.PostAsync("どのようなことでお困りでしょうか？文章で質問を入力してください。");
                context.Call(new BasicQnAMakerDialog(), QnaResumeAfterDialog);
            }
            else if((string)selectedMenu == "終了")
            {
                await context.PostAsync("ご利用ありがとうございました。最後にアンケートをお願いできますか？");
                context.Call(new EnqueteDialog(), EnqueteResumeAfterDialog);
            }
            
        }

        private async Task QnaResumeAfterDialog(IDialogContext context, IAwaitable<object> result) 
        {
            FeedbackMessage(context);
        }
        
        private void FeedbackMessage(IDialogContext context)
        {
            PromptDialog.Choice(context, FeedbackDialog, feedbackList, "解決しましたか？");
        }


        private async Task FeedbackDialog(IDialogContext context, IAwaitable<object> result)
        {
            var feedbackMenu = await result;
            
            if((string)feedbackMenu == "はい")
            {
                await context.PostAsync("ご利用ありがとうございました。");
                MenuMessage(context);
            }
            else if((string)feedbackMenu == "いいえ")
            {
                await context.PostAsync("どのような回答をご希望でしたか？");
                context.Wait(InputMessage);
            }
            
        }

        private async Task EnqueteResumeAfterDialog(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync($"ご協力、ありがとうございました。");

            MenuMessage(context); 
        }
        
        public static string GetSetting(string key)
        {
            var value = Utils.GetAppSetting(key);
            if (String.IsNullOrEmpty(value) && key == "QnAAuthKey")
            {
                value = Utils.GetAppSetting("QnASubscriptionKey"); // QnASubscriptionKey for backward compatibility with QnAMaker (Preview)
            }
            return value;
        }
        
    }
    
    
    /**********************************************************************************/
    // Dialog for QnAMaker GA service
    [Serializable]
    public class BasicQnAMakerDialog : QnAMakerDialog
    {
        // Go to https://qnamaker.ai and feed data, train & publish your QnA Knowledgebase.
        // Parameters to QnAMakerService are:
        // Required: qnaAuthKey, knowledgebaseId, endpointHostName
        // Optional: defaultMessage, scoreThreshold[Range 0.0 – 1.0]
        public BasicQnAMakerDialog() : base(new QnAMakerService(new QnAMakerAttribute(RootDialog.GetSetting("QnAAuthKey"), Utils.GetAppSetting("QnAKnowledgebaseId"), "No good match in FAQ.", 0, 5, Utils.GetAppSetting("QnAEndpointHostName"))))
        { }
        
        //Question from user => message.Text  Answer from bot => result.Answers.FirstOrDefault().Answer
        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            if (message.Text.Equals("上記のどれでもない。"))
            {    
                await context.PostAsync("お力になれず、申し訳ありませんでした。");
            }

            await base.DefaultWaitNextMessageAsync(context, message, result);
        }
    }
    /**********************************************************************************/
    
    [Serializable]
    public class EnqueteDialog : IDialog<string>
    {
        private List<string> menuList = new List<string>() { "1.大満足", "2.満足", "3.普通", "4.不満", "5.とても不満" };

        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(context, this.SelectDialog, this.menuList, "このサービスはいかがでしたか？");
            return Task.CompletedTask;
        }

        private async Task SelectDialog(IDialogContext context, IAwaitable<object> result)
        {
            var selectedMenu = await result;
            context.Done(selectedMenu);
        }
    }
    
    /**********************************************************************************/
}