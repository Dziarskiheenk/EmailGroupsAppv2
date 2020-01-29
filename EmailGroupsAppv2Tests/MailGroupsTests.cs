using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Moq;
using EmailGroupsAppv2.Models;
using EmailGroupsAppv2.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EmailGroupsAppv2Tests
{
  [TestClass]
  public class MailGroupsTests
  {
    [TestMethod]
    public async Task Create_Mail_Group()
    {
      //arrange
      var data = new List<MailGroup>().AsQueryable();
      var newObject = new MailGroup { Name = "1" };
      string userId = "example-user-id";

      var mockMailGroups = GetMock(data);
      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      mockContext.Setup(x => x.SaveChangesAsync()).Returns(() => Task.Run(() => { return 0; })).Verifiable();
      var testUserAccessor = new TestUserAccessor(userId);
      var service = new MailGroupsController(mockContext.Object, testUserAccessor);

      //act
      await service.PostMailGroup(newObject);

      //assert
      mockMailGroups.Verify(x => x.Add(It.IsAny<MailGroup>()), Times.Once);
      mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Get_Mail_Groups()
    {
      //arrange
      string userId = "example-user-id";
      var data = new List<MailGroup>
      {
        new MailGroup{Name="2", OwnerId=userId},
        new MailGroup{Name="3", OwnerId=userId},
        new MailGroup{Name="1", OwnerId=userId},
        new MailGroup{Name="4", OwnerId="wrong-user-id"}
      }.AsQueryable();

      var mockMailGroups = GetMock(data);
      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      var testUserAccessor = new TestUserAccessor(userId);
      var service = new MailGroupsController(mockContext.Object, testUserAccessor);

      //act
      var response = await service.GetMailGroups();

      //assert
      Assert.IsNotNull(response.Value);
      Assert.AreEqual(response.Value.Count(), 3);
      Assert.AreEqual(response.Value.ElementAt(0).Name, "1");
      Assert.AreEqual(response.Value.ElementAt(1).Name, "2");
      Assert.AreEqual(response.Value.ElementAt(2).Name, "3");
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    public async Task Get_Mail_Group(int id)
    {
      //arrange
      string userId = "example-user-id";
      var data = new List<MailGroup>
      {
        new MailGroup{Id=1, OwnerId=userId},
        new MailGroup{Id=2, OwnerId="wrong=user-id"},
        new MailGroup{Id=3, OwnerId=userId}
      }.AsQueryable();

      var mockMailGroups = GetMock(data);
      mockMailGroups.Setup(x => x.FindAsync(id)).ReturnsAsync(() =>
      {
        return mockMailGroups.Object.FirstOrDefault(x => x.Id == id);
      });
      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      var service = new MailGroupsController(mockContext.Object, new TestUserAccessor(userId));

      //act
      var response = await service.GetMailGroup(id);

      //assert
      if (data.Any(x => x.Id == id && x.OwnerId == userId))
      {
        Assert.IsNotNull(response.Value);
        Assert.AreEqual(response.Value.Id, id);
      }
      else
      {
        Assert.IsNull(response.Value);
      }
    }

    [TestMethod]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    public async Task Delete_Mail_Group(int id)
    {
      //arrange
      string userId = "example-user-id";
      var data = new List<MailGroup>
      {
        new MailGroup{Id=1, OwnerId=userId},
        new MailGroup{Id=2, OwnerId="wrong=user-id"},
        new MailGroup{Id=3, OwnerId=userId}
      }.AsQueryable();

      var mockMailGroups = GetMock(data);
      mockMailGroups.Setup(x => x.FindAsync(id)).ReturnsAsync(() =>
      {
        return mockMailGroups.Object.FirstOrDefault(x => x.Id == id);
      });
      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      var service = new MailGroupsController(mockContext.Object, new TestUserAccessor(userId));

      //act
      var response = await service.DeleteMailGroup(id);

      //assert
      if (data.Any(x => x.Id == id && x.OwnerId == userId))
      {
        mockMailGroups.Verify(x => x.Remove(It.IsAny<MailGroup>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.IsNotNull(response.Value);
      }
      else
      {
        mockMailGroups.Verify(x => x.Remove(It.IsAny<MailGroup>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Never);
        Assert.IsNull(response.Value);
      }
    }

    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(2, 2)]
    [DataRow(3, 4)]
    [DataRow(4, 4)]
    public async Task Update_Mail_Group(int routeId, int idOfChangedObject)
    {
      //arrange
      string userId = "example-user-id";
      var data = new List<MailGroup>
      {
        new MailGroup{Id=1, OwnerId=userId},
        new MailGroup{Id=2, OwnerId="wrong=user-id"},
        new MailGroup{Id=3, OwnerId=userId}
      }.AsQueryable();
      var newObject = new MailGroup { Id = idOfChangedObject, Name = "1" };
      var mockMailGroups = GetMock(data);

      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      var service = new MailGroupsController(mockContext.Object, new TestUserAccessor(userId));

      //act
      await service.PutMailGroup(routeId, newObject);

      //assert
      if (routeId == idOfChangedObject && data.Any(x => x.Id == idOfChangedObject && x.OwnerId == userId))
      {
        mockContext.Verify(x => x.MarkMailGroupAsModified(It.IsAny<MailGroup>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
      }
      else
      {
        mockContext.Verify(x => x.MarkMailGroupAsModified(It.IsAny<MailGroup>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Never);
      }
    }

    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(1, 2)]
    [DataRow(2, 2)]
    public async Task Create_Mail_Address(int groupId, int addressGroupId)
    {
      //arrange
      var testMailAddresses = new List<MailAddress>().AsQueryable();
      string userId = "example-user-id";
      var testMailGroups = new List<MailGroup>
      {
        new MailGroup {Id=groupId, OwnerId = userId},
        new MailGroup {Id=groupId, OwnerId = "wrong-user-id"}
      }.AsQueryable();
      var newObject = new MailAddress { GroupId = addressGroupId, Name = "1" };

      var mockMailAddresses = GetMock(testMailAddresses);
      var mockMailGroups = GetMock(testMailGroups);
      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailAddresses).Returns(mockMailAddresses.Object);
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      var service = new MailGroupsController(mockContext.Object, new TestUserAccessor(userId));

      //act
      await service.PostMailAddress(groupId, newObject);

      //assert
      if (groupId == addressGroupId && testMailGroups.Any(x => x.Id == addressGroupId && x.OwnerId == userId))
      {
        mockMailAddresses.Verify(x => x.Add(It.IsAny<MailAddress>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
      }
      else
      {
        mockMailAddresses.Verify(x => x.Add(It.IsAny<MailAddress>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Never);
      }
    }

    [TestMethod]
    [DataRow(1, 1, 1)]
    [DataRow(2, 2, 2)]
    [DataRow(2, 1, 3)]
    public async Task Update_Mail_Address(int groupId, int id, int mailAddressGroupId)
    {
      //arrange
      string userId = "example-user-id";
      var data = new List<MailGroup>
      {
        new MailGroup{Id=1, OwnerId=userId},
        new MailGroup{Id=2, OwnerId="wrong=user-id"},
        new MailGroup{Id=3, OwnerId=userId}
      }.AsQueryable();
      var newObject = new MailAddress { Id = id, GroupId = mailAddressGroupId };
      var mockMailGroups = GetMock(data);

      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockMailGroups.Object);
      var service = new MailGroupsController(mockContext.Object, new TestUserAccessor(userId));

      //act
      await service.PutMailAddress(groupId, id, newObject);

      //assert
      if (groupId == mailAddressGroupId && data.Any(x => x.Id == groupId && x.OwnerId == userId))
      {
        mockContext.Verify(x => x.MarkMailAddressAsModified(It.IsAny<MailAddress>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
      }
      else
      {
        mockContext.Verify(x => x.MarkMailAddressAsModified(It.IsAny<MailAddress>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Never);
      }
    }

    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(1, 2)]
    [DataRow(1, 4)]
    [DataRow(3, 1)]
    [DataRow(2, 1)]
    public async Task Delete_Mail_Address(int groupId, int id)
    {
      //arrange
      string userId = "example-user-id";
      var testMailGroups = new List<MailGroup>
      {
        new MailGroup{Id=1,OwnerId=userId},
        new MailGroup{Id=2,OwnerId="wrong-user-id"},
        new MailGroup{Id=3,OwnerId=userId}
      }.AsQueryable();
      var testMailAddresses = new List<MailAddress>
      {
        new MailAddress{Id=1, GroupId=1},
        new MailAddress{Id=2, GroupId=1},
        new MailAddress{Id=3, GroupId=1}
      }.AsQueryable();

      var mockmailGroups = GetMock(testMailGroups);
      var mockMailAddresses = GetMock(testMailAddresses);
      mockMailAddresses.Setup(x => x.FindAsync(id)).ReturnsAsync(() =>
      {
        return mockMailAddresses.Object.FirstOrDefault(x => x.Id == id);
      });
      var mockContext = new Mock<TestApplicationDbContext>();
      mockContext.Setup(x => x.MailGroups).Returns(mockmailGroups.Object);
      mockContext.Setup(x => x.MailAddresses).Returns(mockMailAddresses.Object);
      var service = new MailGroupsController(mockContext.Object, new TestUserAccessor(userId));

      //act
      var response = await service.DeleteMailAddress(groupId, id);

      //assert
      if (testMailAddresses.Any(x => x.Id == id && x.GroupId == groupId) && testMailGroups.Any(x => x.Id == groupId && x.OwnerId == userId))
      {
        mockMailAddresses.Verify(x => x.Remove(It.IsAny<MailAddress>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.IsNotNull(response.Value);
      }
      else
      {
        mockMailAddresses.Verify(x => x.Remove(It.IsAny<MailAddress>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(), Times.Never);
        Assert.IsNull(response.Value);
      }
    }

    private Mock<DbSet<TEntity>> GetMock<TEntity>(IQueryable<TEntity> data) where TEntity : class
    {
      var enumerable = new TestAsyncEnumerable<TEntity>(data);
      var mockMailGroups = new Mock<DbSet<TEntity>>();
      mockMailGroups.As<IAsyncEnumerable<TEntity>>()
        .Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
        .Returns(enumerable.GetAsyncEnumerator());
      mockMailGroups.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(enumerable.Provider);
      mockMailGroups.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(data.Expression);
      mockMailGroups.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(enumerable.ElementType);
      mockMailGroups.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());
      mockMailGroups.Setup(x => x.AsQueryable()).Returns(mockMailGroups.Object);
      mockMailGroups.Setup(x => x.AsAsyncEnumerable()).Returns(mockMailGroups.Object);
      return mockMailGroups;
    }
  }
}
