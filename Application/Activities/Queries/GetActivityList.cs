using System;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityList
{
    /*
        How this works is we have a kind of Mediator as a Service 
        (MAAS, maybe) that gets the query that's passed in by
        implementing the IRequest interface. From there, it handles 
        the request it received in the nested class Handler, 
        then returns a list of Activities from the database.  
    */
    public class Query : IRequest<List<Activity>> { }

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<Activity>>
    {
        public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            
            return await context.Activities.ToListAsync(cancellationToken);
        }
    }
}
