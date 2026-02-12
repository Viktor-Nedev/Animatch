namespace Animatch.Common
{
	public static class DataValidation
	{
		public static class Animal
		{
			public const int NameMinLength = 2;
			public const int NameMaxLength = 100;

			public const int SpeciesMinLength = 2;
			public const int SpeciesMaxLength = 200;

			public const int BreedMinLength = 2;
			public const int BreedMaxLength = 200;

			public const int TownMinLength = 2;
			public const int TownMaxLength = 100;

			public const int DescriptionMinLength = 20;
			public const int DescriptionMaxLength = 1000;

		}


		public static class Category
		{
			public const int NameMinLength = 2;
			public const int NameMaxLength = 100;
		}

	}
}
