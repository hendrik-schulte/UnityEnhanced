using NodeSystem;
using Nodes.Input;
using Nodes.Logic;
using Nodes.VectorOperation;
using Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Umail
{
    /// <summary>
    /// UI component for sending emails.
    /// </summary>
    public class EmailForm : MonoBehaviour
    {
        public InputField from, to, subject, body, smtp, user, password;
        public Toggle attachment;

        public void Send()
        {
//		Email.SendEmail(from.text, to.text, subject.text, body.text, smtp.text, user.text, password.text);

            var graph = new NodeGraph();
            Node.Instantiate<MathNode>(graph);
            Node.Instantiate<InputNode<float>>(graph);
            Node.Instantiate<CrossNode>(graph);
            Node.Instantiate<DotNode>(graph);

            if (attachment.isOn)
            {
                Email.SendCGMedienMail(to.text, subject.text, body.text, new byte[0]);
//            Email.SendCGMedienMail(to.text, subject.text, body.text, SaveLoad.SerializeToByteArray(graph));
            }
            else
            {
                Email.SendCGMedienMail(to.text, subject.text, body.text);
            }

//        Email.SendEmailToken(
//            "cg.medien@hs-duesseldorf.de",        //from
//            to.text,           //to
//            subject.text,    //subject
//            body.text,        //body
//            token);            //token
        }
    }
}