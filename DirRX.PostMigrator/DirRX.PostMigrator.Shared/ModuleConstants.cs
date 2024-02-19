using System;
using Sungero.Core;

namespace DirRX.PostMigrator.Constants
{
	public static class Module
	{
    /// <summary> Разделитель. </summary>
    public const string Separator = ";";
    /// <summary> Размер блока сущностей для обработки в постмиграции. </summary>
		public const int BlockSize = 200;
    
    /// <summary>
    /// Колонка с признаком пересохранения записи справочника.
    /// </summary>
    public const string Resaved = "resaved";    
		
		public static class Discriminators
		{
			/// <summary> Дискриминатор "Наши организации". </summary>
			[Public]
			public static readonly Guid BusinessUnitDiscriminator = Guid.Parse("EFF95720-181F-4F7D-892D-DEC034C7B2AB");
			
			/// <summary> Дискриминатор "Подразделение". </summary>
			[Public]
			public static readonly Guid DepartmentDiscriminator = Guid.Parse("61b1c19f-26e2-49a5-b3d3-0d3618151e12");
			
			/// <summary> Дискриминатор "Персона". </summary>
			[Public]
			public static readonly Guid PersonDiscriminator = Guid.Parse("f5509cdc-ac0c-4507-a4d3-61d7a0a9b6f6");
			
			/// <summary> Дискриминатор "Учетная запись". </summary>
			[Public]
			public static readonly Guid LoginDiscriminator = Guid.Parse("55F542E9-4645-4F8D-999E-73CC71DF62FD");
			
			/// <summary> Дискриминатор "Роли". </summary>
			[Public]
			public static readonly Guid RoleDiscriminator = Guid.Parse("5E9B55C2-C1F5-4E4F-AA09-7B6D461D87ED");
			
			/// <summary> Дискриминатор "Сотрудники". </summary>
			[Public]
			public static readonly Guid EmployeeDiscriminator = Guid.Parse("b7905516-2be5-4931-961c-cb38d5677565");
			
			/// <summary> Дискриминатор "Сертификат". </summary>
			[Public]
			public static readonly Guid CertificateDiscriminator = Guid.Parse("AFA0C3AA-50FF-453E-87A1-39A696F8917B");
			
			/// <summary> Дискриминатор "Группа Регистрации". </summary>
			[Public]
			public static readonly Guid RegistrationGroupDiscriminator = Guid.Parse("6eddf9a4-bf7b-4cac-961b-a1f0f0bc7ab6");
		}
		
	}
}