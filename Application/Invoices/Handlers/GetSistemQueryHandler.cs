﻿using Application.Common.Interfaces;
using Application.Invoices.Queries;
using Application.Invoices.ViewModels;
using DocumentFormat.OpenXml.Bibliography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invoices.Handlers
{
    public class GetSistemQueryHandler : IRequestHandler<GetSistemQuery, IList<PeopleVM>>
    {
      

        private readonly IApplicationDbContext _context;
        

        public GetSistemQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<PeopleVM>> Handle(GetSistemQuery request, CancellationToken cancellationToken)
        {


            var peoplet = await _context.Peoples.Include(i => i.Contacts)
                .Where(i => i.CreatedBy == request.User).ToListAsync();
            //  var tr=_context.Contacts.Where(a => a.Location == request.lon).FirstOrDefault();
            var vm = peoplet.Select(i => new PeopleVM
            {

                Name=i.Name,

                LastName = i.LastName,
                Company = i.Company,
                Contacts = i.Contacts.Select(k => new ContactVM
                {

                    Phone = k.Phone,
                    Location = k.Location,
                    Email = k.Email,
                    Addrees = k.Addrees,




                }).Where(a => a.Location == request.lon).OrderByDescending(s => s.Location).ToList()

            }).ToList();



            return vm;
        }
    }

}
