using System.Data.Common;
using System.Net.Mime;
using DocumentDataAPI.Data.Repositories;
using DocumentDataAPI.Exceptions;
using DocumentDataAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentDataAPI.Controllers;

[ApiController]
[Route(RoutePrefixHelper.Prefix + "/bias_word_count")]
[Produces(MediaTypeNames.Application.Json)]
public class BiasWordCountController : ControllerBase
{
    private readonly ILogger<BiasWordCountController> _logger;
    private readonly IBiasWordCountRepository _repository;

    public BiasWordCountController(ILogger<BiasWordCountController> logger, IBiasWordCountRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    
}