﻿using Layex.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Layex.Layouts
{
    internal sealed class LayoutManager : ILayoutManager
    {
        private const string LayoutFileExtension = ".layout";
        private const string AttachmentFileExtension = ".attachment";

        private readonly IBootstrapperEnvironment _environment;
        private readonly ILayoutReader _layoutReader;

        public LayoutManager(IBootstrapperEnvironment environment, ILayoutReader layoutReader)
        {
            _environment = environment;
            _layoutReader = layoutReader;
        }

        public Layout LoadLayout(string viewModelCodeName)
        {
            Layout layout = FindLayout(viewModelCodeName);
            List<Attachment> attachments = FindAttachments(viewModelCodeName);
            layout = MergeLayout(layout, attachments);
            return layout;
        }

        private Layout MergeLayout(Layout layout, List<Attachment> attachments)
        {
            //TODO: handle LayoutItems duplication
            List<LayoutItem> layoutItems = new List<LayoutItem>();
            layoutItems.AddRange(layout);
            layoutItems.AddRange(attachments.SelectMany(x => x));
            layoutItems.Sort((x, y) => x.Order - y.Order);
            return new Layout(layout.DisplayMode, layoutItems);
        }

        private Layout FindLayout(string layoutFullName)
        {
            string layoutFileName = layoutFullName + LayoutFileExtension;
            string layoutFile = _environment.FindFile(layoutFileName);
            Layout layout = null;
            if (string.IsNullOrEmpty(layoutFile))
            {
                layout = new Layout();
            }
            else
            {
                string layoutContent = File.ReadAllText(layoutFile);
                layout = _layoutReader.ReadLayout(layoutContent);
            }
            return layout;
        }

        private List<Attachment> FindAttachments(string layoutFullName)
        {
            List<Attachment> attachments = new List<Attachment>();
            string attachmentFileName = layoutFullName + AttachmentFileExtension;
            IEnumerable<string> attachmentFiles = _environment.FindFiles(attachmentFileName);
            foreach (string attachmentFile in attachmentFiles)
            {
                string attachmentContent = File.ReadAllText(attachmentFile);
                Attachment attachment = _layoutReader.ReadAttachment(attachmentContent);
                attachments.Add(attachment);
            }
            return attachments;
        }
    }
}