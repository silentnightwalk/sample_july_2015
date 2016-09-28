using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MetrologyAdmin.FakeData
{
    public class OrganizationsMock : SingletonBase<OrganizationsMock>
    {
        private OrganizationsMock()
        {
            LoadData1();
            LoadData2();
        }

        private List<Organization> Organizations1 = new List<Organization>();
        private List<Organization> Organizations2 = new List<Organization>();

        private Organization[] ListToTree(List<Organization> organizations, params int[] rootIndexs)
        {
            var roots = rootIndexs
                .Where(index => organizations.FirstOrDefault(x => x.Id == index) != null )
                .Select( index => organizations.First(x => x.Id == index))
                .ToArray();

            var levelOrgs = roots;

            var grouped = organizations.GroupBy(o => o.ParentId);

            foreach (var group in grouped)
            {
                var groupHead = organizations.FirstOrDefault(x => x.Id == group.Key);

                if (groupHead != null)
                    groupHead.Children = group.ToArray();

                foreach (var member in group)
                {
                    member.Parent = groupHead;
                }
            }

            return roots;
        }


        public Organization[] GetOrganizationTree(int serverId)
        {
            Thread.Sleep(500);
            if (serverId == 1)
            {
                return ListToTree(LoadData1(), 1, 2);
            }
            else
            {
                return ListToTree(LoadData2(), 1);
            }
        }

        private List<Organization> LoadData1()
        {
            var o1 = new Organization()
            {
                Id = 1,
                Name = "Локомотивное депо",
                ParentId = null,
            };
            var o2 = new Organization()
            {
                Id = 2,
                Name = "Трамвайное депо",
                ParentId = null,
            };
            var o3 = new Organization()
            {
                Id = 3,
                Name = "ООО Важное Дело",
                ParentId = 1,
            };
            var o4 = new Organization()
            {
                Id = 4,
                Name = "ЗАО Инструментарий",
                ParentId = 2,
            };

            var result = new List<Organization>();
            result.AddRange(new Organization[] { o1, o2, o3, o4 });
            return result;
        }

        private List<Organization> LoadData2()
        {
            var o1 = new Organization()
            {
                Id = 1,
                Name = "Первый круг",
                ParentId = null,
            };
            var o2 = new Organization()
            {
                Id = 2,
                Name = "Второе приближение",
                ParentId = 1,
            };
            var o3 = new Organization()
            {
                Id = 3,
                Name = "Третяя ступень",
                ParentId = 2,
            };

            var result = new List<Organization>();
            result.AddRange(new Organization[] { o1, o2, o3 });
            return result;
        }
    }
}
