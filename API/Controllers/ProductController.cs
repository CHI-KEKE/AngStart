using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Amazon.S3.Model;
using API.Domain;
using API.Domain.IService;
using API.Dtos;
using API.Dtos.Product;
using API.Dtos.S3Dto;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("[controller]")]
    // [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly StoreContext _context;
        public IMapper _mapper { get; }
        private readonly IProductRepository _productsRepo;

        private readonly IStorageService _storageService;

        private readonly IConfiguration _config;

        private readonly ProductService _productService;

        public ProductController(StoreContext context,IMapper mapper,ProductService productService,IProductRepository productsRepo,IStorageService storageService,IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _productsRepo = productsRepo;
            _storageService = storageService;
            _config = config;
            _productService = productService;

        }

        [HttpPost("S3ObjUpload")]
        public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> Images)
        {
            S3ResponseDto result = new S3ResponseDto();
            foreach(var Image in Images)
            {

                await using var memoryStr = new MemoryStream();
                await Image.CopyToAsync(memoryStr);

                var fileExt = Path.GetExtension(Image.Name);
                var originalFileName = Path.GetFileNameWithoutExtension(Image.FileName);

                var objName = $"{originalFileName}.{fileExt}";


                var s3Obj = new S3Obj(){
                    BucketName = "thefirstbucket001",
                    InputStream = memoryStr,
                    Name = objName,
                };
                
                var cred = new AWSCredentials()
                {
                    AwsKey = Environment.GetEnvironmentVariable("AwsKey"),
                    AwsSecretKey = Environment.GetEnvironmentVariable("AwsSecretKey")
                };


                result = await _storageService.UploadFileAsync(s3Obj,cred);

            }

            return Ok(new ApiResponse(result.StatusCode,result.Message));
        }

        /// GetAllProducts

        [HttpGet("products")]
        // [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams ProductParams)
        {
            
            var ProductData = await _productsRepo.GetProductsAsync(ProductParams);

            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(ProductData.CurrentPageProducts);  //the final data

            var RealTotalPages = (int)Math.Ceiling(ProductData.TotalPageNumber);

            var (statusCode,IfNextPaging) = _productService.CheckPages(RealTotalPages,ProductParams);

            Pagination<ProductToReturnDto> PageProductList = new Pagination<ProductToReturnDto>(ProductParams.paging,ProductParams.PageSize,ProductData.CountAfterFiltering,data);


            if(statusCode == 400)
            {
               return BadRequest(new ApiResponse(400,"Page Index is Invalid"));
            }

            if(statusCode == 200 && IfNextPaging)
            {
                PageProductList.NextPaging = ProductParams.paging+1;
                return Ok(new ApiResponse(200,"Get Products with nextpaging",PageProductList));               
            }


                return  Ok(new ApiResponse(200,"Get Products with nextpaging",PageProductList));
            
                  
        }


        [HttpGet("products/{id}")]
        // [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductByIdAsync(long id)
        {
            var FoundProduct = await _productsRepo.GetProductByIdAsync(id);
            if (FoundProduct == null) return NotFound();


            return  _mapper.Map<Product,ProductToReturnDto>(FoundProduct);

        }

///////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("Create")]
        // [HttpPost("api/products")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductReceiveDto productReceiveDto)
        {


            var data = _mapper.Map<ProductReceiveDto,Product>(productReceiveDto);


            // var product = new Product
            // {
            //     Category = productReceiveDto.Category,
            //     Title = productReceiveDto.Title,
            //     Description = productReceiveDto.Description,
            //     Price = productReceiveDto.Price,
            //     Texture = productReceiveDto.Texture,
            //     Wash = productReceiveDto.Wash,
            //     Place = productReceiveDto.Place,
            //     Note = productReceiveDto.Note,
            //     Story = productReceiveDto.Story,
            //     // MainImage = formModel.MainImage,
            //     Images = new List<Image>(),
            //     Colors = productReceiveDto.VariantColors.Select((colorCode,index) => new Core.Entities.Color
            //     { 
            //         Code = colorCode,
            //         Name = productReceiveDto.VariantColorNames[index], 

            //     }).ToList(),

            //     Sizes = productReceiveDto.VariantSizes.Select(size => new Size { SizeMark = size }).ToList(),
            //     Variants = productReceiveDto.VariantStocks.Select((stock, index) => new Core.Entities.Variant
            //     {
            //         ColorCode = productReceiveDto.VariantColors[index],
            //         Size = productReceiveDto.VariantSizes[index],
            //         Stock = stock
            //     }).ToList()
            // };



            //Handle Main Images
            
            // Random random = new Random();

            // if (productReceiveDto.MainImage != null && productReceiveDto.MainImage.Length > 0)
            // {
            //     // var mainImagePath = SaveUploadedFile.SaveUploadedFileMethod(formModel.MainImage); // Save the file and get the file path
            //     // product.MainImage = mainImagePath;

            // }



            //Handle Extra Images

            // if (productReceiveDto.Images != null && productReceiveDto.Images.Count > 0)
            // {
            //     foreach (var imageFile in productReceiveDto.Images)
            //     {
            //         var imagePath = SaveUploadedFile.SaveUploadedFileMethod(imageFile); // Save the file and get the file path
            //         var image = new Image { Url = imagePath };
            //         Console.WriteLine(image);
            //         product.Images.Add(image);
            //     }
            // }




            // Handle images
            // var images = new List<Image>();
            // foreach (var formFile in formModel.Images)
            // {
            //     if (formFile.Length > 0)
            //     {
            //         using (var stream = new MemoryStream())
            //         {
            //             await formFile.CopyToAsync(stream);
            //             var image = new Image { Url = Convert.ToBase64String(stream.ToArray()) };
            //             images.Add(image);
            //         }
            //     }
            // }
            // product.Images = images;



            _context.Products.Add(data);
            _context.SaveChanges();

            return Ok(new { status = "nice reqest" });
        }

    }
}