using System;
using System.Collections.Generic;
using System.Data;

using Acesoft.Data;
using Acesoft.Data.Sql;
using Acesoft.Rbac.Entity;
using Acesoft.Util;

namespace Acesoft.Rbac.Schema
{
    public class RbacSchema : IStoreSchema
    {
        public void CreateSchema(ISession session)
        {
            session.BeginTransaction();

            try
            {
                new SchemaBuilder(session)
                    .CreateTable("rbac_scale", t => t.PrimaryKey()
                        .Column<long>("parentid")
                        .Column("ref_id", DbType.AnsiString, c => c.WithLength(50))
                        .Column<string>("name", c => c.NotNull().WithLength(50))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate")
                    )
                    .CreateTable("rbac_user", t => t.PrimaryKey()
                        .Column<long>("scale_id", c => c.NotNull())
                        .Column<long>("client_id")
                        .Column<string>("loginname", c => c.WithLength(20))
                        .Column<string>("nickname", c => c.WithLength(20))
                        .Column<string>("username", c => c.WithLength(20))
                        .Column<string>("password", c => c.WithLength(50))
                        .Column<string>("creator", c => c.WithLength(20))
                        .Column<string>("refcode", c => c.WithLength(20))
                        .Column<string>("weunionid", c => c.WithLength(50))
                        .Column<int>("usertype", c => c.WithDefault(0))
                        .Column<int>("regtype", c => c.WithDefault(0))
                        .Column<bool>("enabled", c => c.WithDefault(true))
                        .Column<string>("mobile", c => c.WithLength(20))
                        .Column<string>("mail", c => c.WithLength(50))
                        .Column<string>("photo", c => c.WithLength(255))
                        .Column<int>("sex")
                        .Column<DateTime>("birthdate")
                        .Column<string>("province", c => c.WithLength(20))
                        .Column<string>("city", c => c.WithLength(20))
                        .Column<string>("county", c => c.WithLength(20))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                        .Column<DateTime>("dupdate")
                        .Column<DateTime>("dlogin")
                        .Column<string>("loginip", c => c.WithLength(20))
                        .Column<bool>("rstpwd")
                        .Column<DateTime>("drstpwd")
                        .Column<int>("trytimes")
                        .Column<string>("remark", c => c.WithLength(255))
                    )
                    .CreateTable("rbac_param", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("value", c => c.WithLength(50))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_role", t => t.PrimaryKey()
                        .Column<long>("scale_id", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<bool>("system", c => c.WithDefault(false))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_ua", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<long>("role_id", c => c.NotNull())
                        .Column<DateTime>("dstart")
                        .Column<DateTime>("dend")
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_object", t => t.PrimaryKey()
                        .Column<long>("parentid", c => c.NotNull())
                        .Column<string>("name", c => c.WithLength(20))
                        .Column<string>("remark", c => c.WithLength(50))
                        .Column<int>("type", c => c.WithDefault(0))
                        .Column<string>("url", c => c.WithLength(255))
                        .Column<string>("icon", c => c.WithLength(50))
                        .Column<string>("opnames", c => c.WithLength(50))
                        .Column<bool>("visible")
                        .Column<int>("orderno", c => c.WithDefault(0))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                    )
                    .CreateTable("rbac_pa", t => t.PrimaryKey()
                        .Column<long>("role_id", c => c.NotNull())
                        .Column<long>("ref_id", c => c.NotNull())
                        .Column<int>("opvalue", c => c.WithDefault(0))
                    )
                    .CreateTable("rbac_auth", t => t.PrimaryKey()
                        .Column<long>("user_id", c => c.NotNull())
                        .Column<string>("authtype", c => c.WithLength(20))
                        .Column<string>("authid", c => c.WithLength(20))
                        .Column<DateTime>("dcreate", c => c.WithDefault("getdate()"))
                        .Column<DateTime>("dupdate")
                    )
                    .CreateForeignKey("fk_auth_user", "rbac_auth", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_param_user", "rbac_param", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_ua_user", "rbac_ua", "user_id", "rbac_user", "id")
                    .CreateForeignKey("fk_user_scale", "rbac_user", "scale_id", "rbac_scale", "id")
                    .CreateForeignKey("fk_role_scale", "rbac_role", "scale_id", "rbac_scale", "id")
                    .CreateForeignKey("fk_ua_role", "rbac_ua", "role_id", "rbac_role", "id")
                    .CreateForeignKey("fk_pa_role", "rbac_pa", "role_id", "rbac_role", "id");

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public void DropSchema(ISession session)
        {
            session.BeginTransaction();

            try
            {
                new SchemaBuilder(session, false)
                .DropForeignKey("rbac_auth", "fk_auth_user")
                .DropForeignKey("rbac_param", "fk_param_user")
                .DropForeignKey("rbac_ua", "fk_ua_user")
                .DropForeignKey("rbac_user", "fk_user_scale")
                .DropForeignKey("rbac_role", "fk_role_scale")
                .DropForeignKey("rbac_ua", "fk_ua_role")
                .DropForeignKey("rbac_pa", "fk_pa_role")
                .DropTable("rbac_auth")
                .DropTable("rbac_pa")
                .DropTable("rbac_object")
                .DropTable("rbac_ua")
                .DropTable("rbac_role")
                .DropTable("rbac_param")
                .DropTable("rbac_user")
                .DropTable("rbac_scale");

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }

        public void InitializeData(ISession session)
        {
            session.BeginTransaction();

            try
            {
                var rootScale = new Rbac_Scale();
                rootScale.InitializeId();
                rootScale.Name = "root";
                rootScale.Remark = "系统顶级，默认请勿删除";
                rootScale.System = true;
                rootScale.DCreate = DateTime.Now;
                session.Insert(rootScale);

                var adminScale = new Rbac_Scale();
                rootScale.Id = Membership.Default_ScaleId;
                rootScale.ParentId = rootScale.Id;
                adminScale.Name = "root";
                adminScale.Remark = "管理员级，最高权限层级";
                adminScale.System = true;
                adminScale.DCreate = DateTime.Now;
                session.Insert(adminScale);

                var roleAdmin = new Rbac_Role();
                roleAdmin.InitializeId();
                roleAdmin.Name = "系统管理员";
                roleAdmin.Scale_Id = rootScale.Id;
                roleAdmin.System = true;
                roleAdmin.DCreate = DateTime.Now;
                session.Insert(roleAdmin);

                var roleGuest = new Rbac_Role();
                roleGuest.InitializeId();
                roleGuest.Name = "游客角色";
                roleGuest.Scale_Id = rootScale.Id;
                roleGuest.System = true;
                roleGuest.DCreate = DateTime.Now;
                session.Insert(roleGuest);

                var root = new Rbac_User();
                root.InitializeId();
                root.Scale_Id = rootScale.Id;
                root.LoginName = "root";
                root.UserName = "超级管理员";
                root.NickName = "管理员";
                root.Password = CryptoHelper.ComputeMD5("root&12345", root.HashId);
                root.DCreate = DateTime.Now;
                session.Insert(root);

                var admin = new Rbac_User();
                admin.InitializeId();
                admin.Scale_Id = adminScale.Id;
                admin.LoginName = "admin";
                admin.UserName = "管理员";
                admin.NickName = "管理员";
                admin.Password = CryptoHelper.ComputeMD5("admin", admin.HashId);
                admin.DCreate = DateTime.Now;
                session.Insert(admin);

                var guest = new Rbac_User();
                guest.InitializeId();
                guest.Scale_Id = rootScale.Id;
                guest.LoginName = "guest";
                guest.UserName = "游客";
                guest.NickName = "游客";
                guest.Password = CryptoHelper.ComputeMD5("guest", guest.HashId);
                guest.DCreate = DateTime.Now;
                session.Insert(guest);

                var ua = new Rbac_UA();
                ua.InitializeId();
                ua.Role_Id = roleAdmin.Id;
                ua.User_Id = admin.Id;
                ua.DCreate = DateTime.Now;
                session.Insert(ua);

                ua = new Rbac_UA();
                ua.InitializeId();
                ua.Role_Id = roleGuest.Id;
                ua.User_Id = guest.Id;
                ua.DCreate = DateTime.Now;
                session.Insert(ua);

                session.Commit();
            }
            catch
            {
                session.Rollback();
                throw;
            }
        }
    }
}
