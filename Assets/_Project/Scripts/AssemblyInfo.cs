using System.Runtime.CompilerServices;

// Cho phép project test gọi các hàm "internal" (ví dụ InitForTests) mà game không thấy.
[assembly: InternalsVisibleTo("NongTrai.Tests.EditMode")]
