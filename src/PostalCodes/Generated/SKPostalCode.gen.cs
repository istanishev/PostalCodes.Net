using System;
using System.Text.RegularExpressions;
using PostalCodes.GenericPostalCodes;

namespace PostalCodes
{
	internal class SKPostalCode : AlphaNumericPostalCode
    {
		public SKPostalCode(string postalCode) : this(postalCode, true) {}

		public SKPostalCode(string postalCode, bool allowConvertToShort) : base(_formats, postalCode, allowConvertToShort) 
		{
			_countryName = "SK";
		}

		private SKPostalCode(PostalCode other) : base(_formats, other.ToString()) {}

        protected override PostalCode PredecessorImpl
        {
			get 
			{ 
				var b = GenerateSuccesorOrPredecessor(GetInternalValue(), false);
				if (b == null) 
				{
					return null;
				}
				return new SKPostalCode (b, _allowConvertToShort);
			}
        }

        protected override PostalCode SuccessorImpl
        {
			get 
			{ 
				var b = GenerateSuccesorOrPredecessor(GetInternalValue(), true);
				if (b == null) 
				{
					return null;
				}
				return new SKPostalCode (b, _allowConvertToShort);
			}
        }

		public override bool Equals (object obj)
		{
			var other = obj as SKPostalCode;
			if (other == null)
				return false;

			return PostalCodeString.Equals (other.PostalCodeString);
		}

		public override int GetHashCode ()
		{
			return PostalCodeString.GetHashCode ();
		}

		public override string ToHumanReadableString ()
		{
			var outputFormat = _currentFormat.OutputDefault;
			if (_currentFormatType == FormatType.Short) 
			{
				if (_currentFormat.OutputShort != null) 
				{
					outputFormat = _currentFormat.OutputShort;
				}
			}

			return ToHumanReadableString(outputFormat);
		}
		
		public override PostalCode ExpandPostalCodeAsLowestInRange ()
		{
			if (_currentFormatType == FormatType.Short) {
				if (_currentFormat.ShortExpansionAsLowestInRange != null) {
					return new SKPostalCode (ToString () + _currentFormat.ShortExpansionAsLowestInRange, false);
				} else {
					throw new ArgumentException ("Requested short postal code expansion but no expansion provided (lowest)");
				}
			}

			return new SKPostalCode(ToString());
		}

		public override PostalCode ExpandPostalCodeAsHighestInRange ()
		{
			if (_currentFormatType == FormatType.Short) {
				if (_currentFormat.ShortExpansionAsHighestInRange != null) {
					return new SKPostalCode (ToString () + _currentFormat.ShortExpansionAsHighestInRange, false);
				} else {
					throw new ArgumentException ("Requested short postal code expansion but no expansion provided (highest)");
				}
			}

			return new SKPostalCode(ToString());
		}

		private static PostalCodeFormat[] _formats = new PostalCodeFormat[] {
			new PostalCodeFormat {
				Name = "5-Digits - 99999",
				RegexDefault = new Regex("^[0-9]{5}$", RegexOptions.Compiled),
				OutputDefault = "xxxxx",
				AutoConvertToShort = false,
				ShortExpansionAsLowestInRange = "0",
				ShortExpansionAsHighestInRange = "9",
				LeftPaddingCharacter = "0",
			}
		};
	}
}
