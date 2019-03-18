
namespace DrNet                                                                                                         
{
    public delegate ref readonly TSource AgregateInRefs<TSource>(in TSource accum, in TSource item);
}
