using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/ObjectDetection", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count < 1)
        return Results.BadRequest();

    using var sceneSourceStream = files[0].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();

    // Your object detection implementation
    var boundingBox = new BoundingBox { X = 50, Y = 50, Width = 100, Height = 100 }; // Example bounding box

    using var image = Image.FromStream(new MemoryStream(imageSceneData));
    using var graphics = Graphics.FromImage(image);
    var pen = new Pen(Color.Red, 3);
    graphics.DrawRectangle(pen, boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height);

    using var resultStream = new MemoryStream();
    image.Save(resultStream, ImageFormat.Jpeg);
    var imageData = resultStream.ToArray();

    return Results.File(imageData, "image/jpg");
}).DisableAntiforgery();

app.Run();

record BoundingBox
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}