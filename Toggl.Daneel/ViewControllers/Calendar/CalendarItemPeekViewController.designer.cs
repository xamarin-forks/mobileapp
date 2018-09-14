// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.ViewControllers.Calendar
{
	[Register ("CalendarItemPeekViewController")]
	partial class CalendarItemPeekViewController
	{
		[Outlet]
		UIKit.UIImageView ImageView { get; set; }

		[Outlet]
		UIKit.UILabel Label { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint LabelLeadingConstraint { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ImageView != null) {
				ImageView.Dispose ();
				ImageView = null;
			}

			if (Label != null) {
				Label.Dispose ();
				Label = null;
			}

			if (LabelLeadingConstraint != null) {
				LabelLeadingConstraint.Dispose ();
				LabelLeadingConstraint = null;
			}
		}
	}
}
