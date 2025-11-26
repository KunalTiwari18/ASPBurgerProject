using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using BBURGERClone.Models;

namespace BBURGERClone.Helpers
{
    public class SessionCart
    {
        private const string SessionKey = "Cart";
        private readonly ISession _session;

        public SessionCart(ISession session)
        {
            _session = session;
        }

        // Internal DTO stored in session to avoid serializing full Product every time
        private class CartDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        private List<CartDto> ReadDto()
        {
            var json = _session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json)) return new List<CartDto>();
            try
            {
                return JsonSerializer.Deserialize<List<CartDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                       ?? new List<CartDto>();
            }
            catch
            {
                return new List<CartDto>();
            }
        }

        private void SaveDto(List<CartDto> dtos)
        {
            var json = JsonSerializer.Serialize(dtos);
            _session.SetString(SessionKey, json);
        }

        // Returns CartItem list but Product property may be null (caller should populate using product service).
        public List<CartItem> GetItems()
        {
            var dtos = ReadDto();
            return dtos.Select(d => new CartItem
            {
                ProductId = d.ProductId,
                Product = null!,        // populate later by caller
                Quantity = d.Quantity
            }).ToList();
        }

        public void AddToCart(Product product, int qty = 1)
        {
            if (product == null) return;
            var dtos = ReadDto();
            var existing = dtos.FirstOrDefault(x => x.ProductId == product.Id);
            if (existing != null)
            {
                existing.Quantity += qty;
            }
            else
            {
                dtos.Add(new CartDto { ProductId = product.Id, Quantity = qty });
            }
            SaveDto(dtos);
        }

        public void RemoveFromCart(int productId)
        {
            var dtos = ReadDto();
            dtos.RemoveAll(x => x.ProductId == productId);
            SaveDto(dtos);
        }

        public void Clear()
        {
            _session.Remove(SessionKey);
        }
    }
}
